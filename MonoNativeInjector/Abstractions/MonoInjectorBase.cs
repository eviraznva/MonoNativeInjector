using System.Diagnostics;
using MonoNativeInjector.Misc;
using MonoNativeInjector.Common;
using static MonoNativeInjector.Misc.Logger;

namespace MonoNativeInjector.Abstractions;

/// <summary>
/// Provides a base implementation for Mono injectors, handling common setup and disposal functionalities.
/// </summary>
internal abstract class MonoInjectorBase : IMonoInjector
{
    // Holds pointers to functions within the Mono module.
    protected readonly Dictionary<string, IntPtr> _functionsPtrs = new();

    // Represents the game process into which assemblies will be injected.
    protected readonly Process _gameProcess;

    // Facilitates memory operations on the game process.
    protected readonly Memory _memory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonoInjectorBase"/> class.
    /// </summary>
    /// <param name="gameProcess">The target game process for injection.</param>
    protected MonoInjectorBase(Process gameProcess)
    {
        _gameProcess = gameProcess;
        
        WaitForMonoModule();
        
        _memory = new Memory(_gameProcess.Id);
    }
    
    public abstract IntPtr Inject(string assemblyPath, string namespaceName, string className, string methodName);
    
    public abstract void Eject(IntPtr assemblyPtr, string namespaceName, string className, string methodName);
    
    public virtual void Dispose()
    {
        _memory.Dispose();
    }
    
    /// <summary>
    /// Waits for the Mono module to be loaded into the game process, essential for the injection process.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the Mono module cannot be found after several attempts.</exception>
    private void WaitForMonoModule()
    {
        var monoModule = _gameProcess.Modules.Cast<ProcessModule>().FirstOrDefault(module => 
            module.ModuleName.Contains("mono", StringComparison.InvariantCultureIgnoreCase));
        
        var retryCount = 0;
        
        while (monoModule is null && retryCount-- < 20)
        {
            LogWarning("Mono module not found, retrying...");
            
            monoModule = _gameProcess.Modules.Cast<ProcessModule>().FirstOrDefault(module => 
                module.ModuleName.Contains("mono", StringComparison.InvariantCultureIgnoreCase));
        }
        
        if (monoModule is null) throw new InvalidOperationException("Mono module not found!");   
        
        Thread.Sleep(1500);
        
        LogInfo("Mono module found!");
        
        MonoModuleBaseAddress = monoModule.BaseAddress;
    }

    // Base address of the Mono module loaded in the game process.
    protected IntPtr MonoModuleBaseAddress { get; private set; } = IntPtr.Zero;

    // Pointer to the root domain of the Mono runtime in the game process.
    protected IntPtr RootDomain { get; set; } = IntPtr.Zero;
}