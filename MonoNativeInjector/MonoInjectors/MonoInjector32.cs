using Iced.Intel;
using System.Text;
using System.Diagnostics;
using MonoNativeInjector.Misc;
using MonoNativeInjector.Structs;
using MonoNativeInjector.Abstractions;
using static Iced.Intel.AssemblerRegisters;

namespace MonoNativeInjector.MonoInjectors;

/// <summary>
/// A Mono injector implementation for 32-bit processes.
/// </summary>
internal sealed class MonoInjector32(Process _gameProcess) : MonoInjectorBase(_gameProcess)
{
    /// <summary>
    /// Injects an assembly into the Mono process.
    /// </summary>
    /// <param name="assemblyPath">The file path of the assembly to inject.</param>
    /// <param name="namespaceName">The namespace containing the class.</param>
    /// <param name="className">The class containing the method to invoke.</param>
    /// <param name="methodName">The method to invoke upon injection.</param>
    /// <returns>A pointer to the injected assembly.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the root domain or assembly image cannot be obtained, or class or method lookup fails.</exception>
    public override IntPtr Inject(string assemblyPath, string namespaceName, string className, string methodName)
    {
        if (RootDomain == IntPtr.Zero) RootDomain = GetRootDomain();
        if (RootDomain == IntPtr.Zero) throw new InvalidOperationException("Failed to get root domain");
        
        Logger.LogDebug($"Root domain: 0x{RootDomain.ToInt32():X}");
        
        var assemblyPtr = OpenMonoAssembly(assemblyPath, out var status);
        
        Logger.LogDebug($"Assembly: 0x{assemblyPtr.ToInt32():X}");
        
        if (assemblyPtr == IntPtr.Zero) throw new InvalidOperationException($"Failed to open assembly: {status}");
        
        var imagePtr = GetMonoAssemblyImage(assemblyPtr);
        
        Logger.LogDebug($"Image: 0x{imagePtr.ToInt32():X}");
        
        if (imagePtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get assembly image");
        
        var classPtr = GetMonoClass(imagePtr, namespaceName, className);
        
        Logger.LogDebug($"Class: 0x{classPtr.ToInt32():X}");
        
        if (classPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get class");
        
        var methodPtr = GetMonoMethod(classPtr, methodName);
        
        Logger.LogDebug($"Method: 0x{methodPtr.ToInt32():X}");
        
        if (methodPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get method");
        
        InvokeMonoMethod(methodPtr);
        
        Logger.LogDebug("Method invoked");
        
        return assemblyPtr;
    }

    /// <summary>
    /// Ejects an assembly from the Mono process.
    /// </summary>
    /// <param name="assemblyPtr">A pointer to the previously injected assembly.</param>
    /// <param name="namespaceName">The namespace of the class.</param>
    /// <param name="className">The class of the method.</param>
    /// <param name="methodName">The method to invoke for ejection.</param>
    /// <remarks>Currently implements the same logic as injection for demonstration. Actual ejection logic may differ based on the Mono runtime's capabilities.</remarks>
    public override void Eject(IntPtr assemblyPtr, string namespaceName, string className, string methodName)
    {
        if (RootDomain == IntPtr.Zero) RootDomain = GetRootDomain();
        if (RootDomain == IntPtr.Zero) throw new InvalidOperationException("Failed to get root domain");
        
        Logger.LogDebug($"Root domain: 0x{RootDomain.ToInt32():X}");
        
        var imagePtr = GetMonoAssemblyImage(assemblyPtr);
        
        Logger.LogDebug($"Image: 0x{imagePtr.ToInt32():X}");
        
        if (imagePtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get assembly image");
        
        var classPtr = GetMonoClass(imagePtr, namespaceName, className);
        
        Logger.LogDebug($"Class: 0x{classPtr.ToInt32():X}");
        
        if (classPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get class");
        
        var methodPtr = GetMonoMethod(classPtr, methodName);
        
        Logger.LogDebug($"Method: 0x{methodPtr.ToInt32():X}");
        
        if (methodPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get method");
        
        InvokeMonoMethod(methodPtr);
        
        Logger.LogDebug("Method invoked");
    }

    /// <summary>
    /// Retrieves the Mono root domain for the process.
    /// </summary>
    /// <returns>A pointer to the Mono root domain.</returns>
    private IntPtr GetRootDomain()
    {
        var getRootDomainFuncPtr = GetMonoFuncAddress("mono_get_root_domain");
        
        if (getRootDomainFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get root domain function address");
        
        var retries = 0;
        
        while (++retries < 10)
        {
            var result = ExecuteAssemblyMethod(getRootDomainFuncPtr);
            
            if (result != IntPtr.Zero) return result;
            
            Logger.LogWarning("Failed to get root domain, retrying...");
            
            Thread.Sleep(1000);
        }
        
        return IntPtr.Zero;
    }

    /// <summary>
    /// Opens a Mono assembly from the specified path.
    /// </summary>
    /// <param name="assemblyPath">The file path of the assembly.</param>
    /// <param name="status">The status code returned by the Mono runtime.</param>
    /// <returns>A pointer to the opened Mono assembly.</returns>
    private IntPtr OpenMonoAssembly(string assemblyPath, out int status)
    {
        var openAssemblyFuncPtr = GetMonoFuncAddress("mono_assembly_open");
        
        if (openAssemblyFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get open assembly function address");
        
        var assemblyPathPtr = _memory.AllocateMemory(assemblyPath.Length + 1);
        var statusPtr = _memory.AllocateMemory(4);
        
        _memory.WriteBytes(assemblyPathPtr, Encoding.ASCII.GetBytes(assemblyPath));
        
        var result = ExecuteAssemblyMethod(openAssemblyFuncPtr, true, assemblyPathPtr, statusPtr);
        
        status = _memory.ReadInt32(statusPtr);
        
        _memory.FreeMemory(assemblyPathPtr);
        
        return result;
    }

    /// <summary>
    /// Gets the Mono image for an assembly.
    /// </summary>
    /// <param name="monoAssembly">A pointer to the Mono assembly.</param>
    /// <returns>A pointer to the Mono image.</returns>
    private IntPtr GetMonoAssemblyImage(IntPtr monoAssembly)
    {
        var getAssemblyImageFuncPtr = GetMonoFuncAddress("mono_assembly_get_image");
        
        if (getAssemblyImageFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get assembly image function address");
        
        return ExecuteAssemblyMethod(getAssemblyImageFuncPtr, true, monoAssembly);
    }

    /// <summary>
    /// Retrieves a Mono class from an assembly image.
    /// </summary>
    /// <param name="monoAssemblyImage">A pointer to the Mono image.</param>
    /// <param name="nameSpace">The namespace of the class.</param>
    /// <param name="className">The name of the class.</param>
    /// <returns>A pointer to the Mono class.</returns>
    private IntPtr GetMonoClass(IntPtr monoAssemblyImage, string nameSpace, string className)
    {
        var getClassFuncPtr = GetMonoFuncAddress("mono_class_from_name");
        
        if (getClassFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get class function address");
        
        var nameSpacePtr = _memory.AllocateMemory(nameSpace.Length + 1);
        var classNamePtr = _memory.AllocateMemory(className.Length + 1);
        
        _memory.WriteBytes(nameSpacePtr, Encoding.ASCII.GetBytes(nameSpace));
        _memory.WriteBytes(classNamePtr, Encoding.ASCII.GetBytes(className));
        
        var result = ExecuteAssemblyMethod(getClassFuncPtr, true, monoAssemblyImage, nameSpacePtr, classNamePtr);
        
        _memory.FreeMemory(nameSpacePtr);
        _memory.FreeMemory(classNamePtr);
        
        return result;
    }

    /// <summary>
    /// Retrieves a Mono method from a class.
    /// </summary>
    /// <param name="monoClass">A pointer to the Mono class.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>A pointer to the Mono method.</returns>
    private IntPtr GetMonoMethod(IntPtr monoClass, string methodName)
    {
        var getMethodFuncPtr = GetMonoFuncAddress("mono_class_get_method_from_name");
        
        if (getMethodFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get method function address");
        
        var methodNamePtr = _memory.AllocateMemory(methodName.Length + 1);
        
        _memory.WriteBytes(methodNamePtr, Encoding.ASCII.GetBytes(methodName));
        
        var result = ExecuteAssemblyMethod(getMethodFuncPtr, true, monoClass, methodNamePtr, 0);
        
        _memory.FreeMemory(methodNamePtr);
        
        return result;
    }

    /// <summary>
    /// Invokes a Mono method.
    /// </summary>
    /// <param name="monoMethod">A pointer to the Mono method.</param>
    private void InvokeMonoMethod(IntPtr monoMethod)
    {
        var invokeMethodFuncPtr = GetMonoFuncAddress("mono_runtime_invoke");
        
        if (invokeMethodFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get invoke method function address");
        
        ExecuteAssemblyMethod(invokeMethodFuncPtr, true, monoMethod, 0, 0, 0);
    }
    
    /// <summary>
    /// Executes a Mono assembly method.
    /// </summary>
    /// <param name="funcAddress">The function address in the Mono runtime.</param>
    /// <param name="attach">Whether to attach the executing thread to the Mono runtime.</param>
    /// <param name="args">Arguments for the function call.</param>
    /// <returns>The result of the method execution.</returns>
    private IntPtr ExecuteAssemblyMethod(IntPtr funcAddress, bool attach = false, params IntPtr[] args)
    {
        var assembler = new Assembler(32);

        var resultMemPtr = _memory.AllocateMemory(4);

        if (attach)
        {
            var attachFuncPtr = GetMonoFuncAddress("mono_thread_attach");
            
            if (attachFuncPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to get attach function address");
            
            assembler.push(RootDomain.ToInt32());
            assembler.mov(eax, attachFuncPtr.ToInt32());
            assembler.call(eax);
            assembler.add(esp, 0x4);
        }
        
        for (var i = args.Length - 1; i >= 0; i--) assembler.push(args[i].ToInt32());
        
        assembler.mov(eax, funcAddress.ToInt32());
        assembler.call(eax);
        
        assembler.add(esp, (byte)(args.Length * 4));
        
        assembler.mov(__[resultMemPtr.ToInt32()], eax);
        
        assembler.ret();
        
        var codeFormatter = new IntelFormatter();
        var stringOutput = new StringOutput();
        
        foreach(var instruction in assembler.Instructions)
        {
            codeFormatter.Format(instruction, stringOutput);
            Logger.LogDebug(stringOutput.ToStringAndReset());
        }
        
        using var memoryStream = new MemoryStream();

        assembler.Assemble(new StreamCodeWriter(memoryStream), 0);
        
        var codeAllocatedMemPtr = _memory.AllocateMemory((int)memoryStream.Length);
        
        _memory.WriteBytes(codeAllocatedMemPtr, memoryStream.ToArray());
        
        _memory.InvokeRemoteThread(codeAllocatedMemPtr);
        
        Logger.LogWarning($"Allocated memory: 0x{codeAllocatedMemPtr.ToInt32():X}");
        Logger.LogWarning($"Allocated memory result: 0x{resultMemPtr.ToInt32():X}");
        
        var result = _memory.ReadIntPtr(resultMemPtr, false);
        
        _memory.FreeMemory(codeAllocatedMemPtr);
        _memory.FreeMemory(resultMemPtr);
        
        return result;
    }
    
    /// <summary>
    /// Gets the address of a function in the Mono runtime by name.
    /// </summary>
    /// <param name="funcName">The name of the function.</param>
    /// <returns>The address of the function, or <see cref="IntPtr.Zero"/> if not found.</returns>
    private IntPtr GetMonoFuncAddress(string funcName)
    {
        if (_functionsPtrs.TryGetValue(funcName, out var address))
        {
            Logger.LogDebug($"Function {funcName} address: 0x{address.ToInt32():X}");
            return address;
        };
        
        var dosHeader = _memory.ReadAndCastToStruct<IMAGE_DOS_HEADER>(MonoModuleBaseAddress);
        
        var ntHeader = _memory.ReadAndCastToStruct<IMAGE_NT_HEADERS>((IntPtr)(MonoModuleBaseAddress + dosHeader.e_lfanew));
        
        var exportDir = _memory.ReadAndCastToStruct<IMAGE_EXPORT_DIRECTORY>((IntPtr)(MonoModuleBaseAddress + ntHeader.OptionalHeader.imageDataExportDirectory.Size));
        
        var names = (IntPtr)(MonoModuleBaseAddress + exportDir.AddressOfNames);
        var ordinals = (IntPtr)(MonoModuleBaseAddress + exportDir.AddressOfNameOrdinals);
        var functions = (IntPtr)(MonoModuleBaseAddress + exportDir.AddressOfFunctions);

        for (var i = 0; i < exportDir.NumberOfNames; i++)
        {
            var offset = _memory.ReadInt32(names + i * 4);
            var name = _memory.ReadString(MonoModuleBaseAddress + offset, Encoding.ASCII);
            var ordinal = _memory.ReadInt16(ordinals + i * 2);
            var functionAddress = MonoModuleBaseAddress + _memory.ReadInt32(functions + ordinal * 4);
               
            _functionsPtrs.TryAdd(name, functionAddress);

            if (name.Contains(funcName, StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.LogDebug($"Function {funcName} address: 0x{address.ToInt32():X}");
                return functionAddress;
            };
        }
        
        return IntPtr.Zero;
    }
}