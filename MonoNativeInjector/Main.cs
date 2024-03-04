using System.Diagnostics;
using MonoNativeInjector.Misc;
using MonoNativeInjector.Common;
using System.Runtime.InteropServices;
using MonoNativeInjector.Abstractions;
using static MonoNativeInjector.Misc.Logger;
using MonoNativeInjector.MonoInjectorCreators;

namespace MonoNativeInjector;

internal static unsafe class Main
{
    internal static delegate* managed<IntPtr, int, void> Logger;

    /// <summary>
    /// Sets the logger function pointer to be used for logging within the application.
    /// </summary>
    /// <param name="logger">A pointer to the managed delegate for logging.</param>
    [UnmanagedCallersOnly(EntryPoint = "SetLogger")]
    public static void SetLogger(IntPtr logger) => Logger = (delegate* managed<IntPtr, int, void>)logger;

    /// <summary>
    /// Opens an instance of MonoInjector based on the process name provided. It tries to find the process up to 5 times before failing.
    /// </summary>
    /// <param name="processNamePtr">Pointer to the process name in memory.</param>
    /// <returns>A handle to the instance of MonoInjector.</returns>
    /// <exception cref="Exception">Thrown when an instance of MonoInjector cannot be created.</exception>
    [UnmanagedCallersOnly(EntryPoint = "OpenMonoInjector")]
    public static IntPtr OpenMonoInjector(IntPtr processNamePtr)
    {
        MonoInjector? monoInjector = null;

        LogInfo("Creating instance of MonoInjector...");

        var processName = Marshal.PtrToStringAnsi(processNamePtr);

        var retryCount = 0;

        while (retryCount < 5)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            
            if (process is null) LogWarning("Process not found, retrying...");
            else
            {
                LogInfo("Process found!");
                
                LogInfo(process.ProcessName);
                
                _ = WindowsNative.IsWow64Process(process.Id, out var isWow64Process);
                
                if (!isWow64Process)
                {
                    LogWarning("Process is 64-bit!");
                    
                    monoInjector = new MonoInjector64Creator(process);
                    
                    break;
                }

                LogWarning("Process is 32-bit!");
                    
                monoInjector = new MonoInjector32Creator(process);
                    
                break;
            }
            
            Thread.Sleep(2000);
            
            ++retryCount;
        }
        
        if (monoInjector is null) throw new Exception("Failed to create instance of MonoInjector!");
        
        var instanceOfMonoInjector = monoInjector.CreateInstanceOfInjector();
        
        LogInfo("Instance of MonoInjector created!");

        return GCHandle.ToIntPtr(GCHandle.Alloc(instanceOfMonoInjector));
    }
    
    /// <summary>
    /// Injects an assembly into the specified namespace and class method within a Mono process.
    /// </summary>
    /// <param name="instanceOfMonoInjector">Handle to the instance of MonoInjector.</param>
    /// <param name="assemblyPathPtr">Pointer to the assembly path in memory.</param>
    /// <param name="namespacePtr">Pointer to the namespace in memory.</param>
    /// <param name="classNamePtr">Pointer to the class name in memory.</param>
    /// <param name="methodNamePtr">Pointer to the method name in memory.</param>
    /// <returns>A pointer indicating the result of the injection process.</returns>
    [UnmanagedCallersOnly(EntryPoint = "Inject")]
    public static IntPtr Inject(IntPtr instanceOfMonoInjector, IntPtr assemblyPathPtr, IntPtr namespacePtr, IntPtr classNamePtr, IntPtr methodNamePtr)
    {
        var monoInjector = (IMonoInjector)GCHandle.FromIntPtr(instanceOfMonoInjector).Target!;
        
        var assemblyPath = Marshal.PtrToStringAnsi(assemblyPathPtr);
        var @namespace = Marshal.PtrToStringAnsi(namespacePtr);
        var className = Marshal.PtrToStringAnsi(classNamePtr);
        var methodName = Marshal.PtrToStringAnsi(methodNamePtr);
        
        LogInfo($"Injecting {assemblyPath} into {className}.{methodName}...");

        var result = monoInjector.Inject(assemblyPath!, @namespace!, className!, methodName!);
        
        LogInfo("Injection completed!");

        return result;
    }
    
    /// <summary>
    /// Ejects an injected assembly from the specified namespace and class method within a Mono process.
    /// </summary>
    /// <param name="instanceOfMonoInjector">Handle to the instance of MonoInjector.</param>
    /// <param name="assemblyPtr">Pointer to the assembly in memory.</param>
    /// <param name="namespaceName">Pointer to the namespace in memory.</param>
    /// <param name="className">Pointer to the class name in memory.</param>
    /// <param name="methodName">Pointer to the method name in memory.</param>
    [UnmanagedCallersOnly(EntryPoint = "Eject")]
    public static void Eject(IntPtr instanceOfMonoInjector, IntPtr assemblyPtr, IntPtr namespaceName, IntPtr className, IntPtr methodName)
    {
        var monoInjector = (IMonoInjector)GCHandle.FromIntPtr(instanceOfMonoInjector).Target!;
        
        LogInfo("Ejecting...");

        monoInjector.Eject(assemblyPtr, Marshal.PtrToStringAnsi(namespaceName)!, Marshal.PtrToStringAnsi(className)!,
            Marshal.PtrToStringAnsi(methodName)!);

        LogInfo("Ejection completed!");
    }
    
    /// <summary>
    /// Closes and disposes of the MonoInjector instance.
    /// </summary>
    /// <param name="instanceOfMonoInjector">Handle to the instance of MonoInjector.</param>
    [UnmanagedCallersOnly(EntryPoint = "CloseMonoInjector")]
    public static void CloseMonoInjector(IntPtr instanceOfMonoInjector)
    {
        var monoInjector = (IMonoInjector)GCHandle.FromIntPtr(instanceOfMonoInjector).Target!;
        
        LogInfo("Disposing...");

        monoInjector.Dispose();

        LogInfo("Disposed!");
    }
}