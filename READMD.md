# MonoNativeInjector

MonoNativeInjector is a powerful tool designed to inject assemblies into Mono processes. It's particularly useful for developers working on modding, reverse engineering, or enhancing Mono-based applications. This document provides an overview of how to use MonoNativeInjector in your projects.

## Features

- Inject assemblies into 32-bit and 64-bit Mono processes.
- Custom logger support for monitoring injection process.
- Easy to use API for injecting, ejecting, and managing Mono assemblies.

## Requirements

- This library is compiled natively and is intended to be used on Windows platforms only.
- Compatible with Windows 64-bit (win64) and Windows 32-bit (win86) systems.
- A target Mono-based application process.

## Getting Started

To start using MonoNativeInjector, ensure you have cloned this repository and built `MonoNativeInjector.dll` according to the instructions provided.

## Build

To build the MonoNativeInjector for specific platforms, use the following commands:

For Windows 64-bit:

dotnet publish -r win-x64

For Windows 32-bit:

dotnet publish -r win-x86

These commands will compile the project for the respective Windows architecture, ensuring compatibility and optimizing performance for the target platform.

### Example Usage

Below is an example demonstrating how to use MonoNativeInjector to inject and eject an assembly into a Mono process. This example includes setting up a custom logger, opening a Mono process, injecting an assembly, and then cleaning up.

```csharp
using System;
using System.Runtime.InteropServices;

unsafe
{
    delegate* managed<IntPtr, int, void> delegateDefinition1 = &DelegateDefinition;

    // Set a custom logger.
    SetLogger((IntPtr)delegateDefinition1);

    // Open a Mono process by name.
    var monoInjector = OpenMonoInjector(Marshal.StringToHGlobalAnsi("ProcessName"));

    try
    {
        // Inject an assembly into the Mono process.
        var injectResult = Inject(monoInjector, Marshal.StringToHGlobalAnsi(@"Path\To\Your\Assembly.dll"), 
            Marshal.StringToHGlobalAnsi("Namespace"), 
            Marshal.StringToHGlobalAnsi("ClassName"), 
            Marshal.StringToHGlobalAnsi("MethodName"));
        
        // Eject the previously injected assembly.
        Eject(monoInjector, injectResult, Marshal.StringToHGlobalAnsi("Namespace"), 
            Marshal.StringToHGlobalAnsi("ClassName"), 
            Marshal.StringToHGlobalAnsi("MethodName"));
    }
    finally
    {
        // Ensure the Mono injector is properly closed.
        CloseMonoInjector(monoInjector);   
    }
    
    return;

    // Custom logger definition.
    static void DelegateDefinition(IntPtr nint, int arg2) => Console.WriteLine($"{arg2}: {Marshal.PtrToStringAnsi(nint)}");
}

// P/Invoke declarations for interacting with MonoNativeInjector.dll.
[DllImport(@"Path\To\MonoNativeInjector.dll", EntryPoint = "SetLogger")]
static extern void SetLogger(IntPtr logger);

[DllImport(@"Path\To\MonoNativeInjector.dll", EntryPoint = "OpenMonoInjector")]
static extern IntPtr OpenMonoInjector(IntPtr processName);

[DllImport(@"Path\To\MonoNativeInjector.dll", EntryPoint = "Inject")]
static extern IntPtr Inject(IntPtr instanceOfMonoInjector, IntPtr assemblyPathPtr, IntPtr namespacePtr,
    IntPtr classNamePtr, IntPtr methodNamePtr);

[DllImport(@"Path\To\MonoNativeInjector.dll", EntryPoint = "Eject")]
static extern void Eject(IntPtr instanceOfMonoInjector, IntPtr assemblyPtr, IntPtr namespaceName, IntPtr className, IntPtr methodName);

[DllImport(@"Path\To\MonoNativeInjector.dll", EntryPoint = "CloseMonoInjector")]
static extern void CloseMonoInjector(IntPtr instanceOfMonoInjector);
```

# Contributing

Contributions to MonoNativeInjector are always welcome. Please feel free to submit pull requests or create issues for bugs, feature requests, or any other feedback.

# Disclaimer

This project is created for educational purposes only. It is not intended to be used for creating cheats in multiplayer games or any other activities that could violate terms of service or fair play guidelines of any game or software. Use of MonoNativeInjector in such a manner is strictly against the intent of the project and the developers do not condone or support misuse of this software.

# License

MIT License

Copyright (c) 2024 Eviraznva

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# Documentation

<a name='assembly'></a>
# MonoNativeInjector

- [IMonoInjector](#T-MonoNativeInjector-Common-IMonoInjector 'MonoNativeInjector.Common.IMonoInjector')
  - [Eject(assemblyPtr,namespaceName,className,methodName)](#M-MonoNativeInjector-Common-IMonoInjector-Eject-System-IntPtr,System-String,System-String,System-String- 'MonoNativeInjector.Common.IMonoInjector.Eject(System.IntPtr,System.String,System.String,System.String)')
  - [Inject(assemblyPath,namespaceName,className,methodName)](#M-MonoNativeInjector-Common-IMonoInjector-Inject-System-String,System-String,System-String,System-String- 'MonoNativeInjector.Common.IMonoInjector.Inject(System.String,System.String,System.String,System.String)')
- [Logger](#T-MonoNativeInjector-Misc-Logger 'MonoNativeInjector.Misc.Logger')
  - [LogDebug(message)](#M-MonoNativeInjector-Misc-Logger-LogDebug-System-String- 'MonoNativeInjector.Misc.Logger.LogDebug(System.String)')
  - [LogInfo(message)](#M-MonoNativeInjector-Misc-Logger-LogInfo-System-String- 'MonoNativeInjector.Misc.Logger.LogInfo(System.String)')
  - [LogWarning(message)](#M-MonoNativeInjector-Misc-Logger-LogWarning-System-String- 'MonoNativeInjector.Misc.Logger.LogWarning(System.String)')
- [Main](#T-MonoNativeInjector-Main 'MonoNativeInjector.Main')
  - [CloseMonoInjector(instanceOfMonoInjector)](#M-MonoNativeInjector-Main-CloseMonoInjector-System-IntPtr- 'MonoNativeInjector.Main.CloseMonoInjector(System.IntPtr)')
  - [Eject(instanceOfMonoInjector,assemblyPtr,namespaceName,className,methodName)](#M-MonoNativeInjector-Main-Eject-System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr- 'MonoNativeInjector.Main.Eject(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr)')
  - [Inject(instanceOfMonoInjector,assemblyPathPtr,namespacePtr,classNamePtr,methodNamePtr)](#M-MonoNativeInjector-Main-Inject-System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr- 'MonoNativeInjector.Main.Inject(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr)')
  - [OpenMonoInjector(processNamePtr)](#M-MonoNativeInjector-Main-OpenMonoInjector-System-IntPtr- 'MonoNativeInjector.Main.OpenMonoInjector(System.IntPtr)')
  - [SetLogger(logger)](#M-MonoNativeInjector-Main-SetLogger-System-IntPtr- 'MonoNativeInjector.Main.SetLogger(System.IntPtr)')
- [Memory](#T-MonoNativeInjector-Misc-Memory 'MonoNativeInjector.Misc.Memory')
  - [#ctor(processId)](#M-MonoNativeInjector-Misc-Memory-#ctor-System-Int32- 'MonoNativeInjector.Misc.Memory.#ctor(System.Int32)')
  - [ProcessHandle](#P-MonoNativeInjector-Misc-Memory-ProcessHandle 'MonoNativeInjector.Misc.Memory.ProcessHandle')
  - [AllocateMemory(size)](#M-MonoNativeInjector-Misc-Memory-AllocateMemory-System-Int32- 'MonoNativeInjector.Misc.Memory.AllocateMemory(System.Int32)')
  - [FreeMemory(address)](#M-MonoNativeInjector-Misc-Memory-FreeMemory-System-IntPtr- 'MonoNativeInjector.Misc.Memory.FreeMemory(System.IntPtr)')
  - [InvokeRemoteThread(address)](#M-MonoNativeInjector-Misc-Memory-InvokeRemoteThread-System-IntPtr- 'MonoNativeInjector.Misc.Memory.InvokeRemoteThread(System.IntPtr)')
  - [ReadAndCastToStruct\`\`1(address)](#M-MonoNativeInjector-Misc-Memory-ReadAndCastToStruct``1-System-IntPtr- 'MonoNativeInjector.Misc.Memory.ReadAndCastToStruct``1(System.IntPtr)')
  - [ReadInt16(address)](#M-MonoNativeInjector-Misc-Memory-ReadInt16-System-IntPtr- 'MonoNativeInjector.Misc.Memory.ReadInt16(System.IntPtr)')
  - [ReadInt32(address)](#M-MonoNativeInjector-Misc-Memory-ReadInt32-System-IntPtr- 'MonoNativeInjector.Misc.Memory.ReadInt32(System.IntPtr)')
  - [ReadInt64(address)](#M-MonoNativeInjector-Misc-Memory-ReadInt64-System-IntPtr- 'MonoNativeInjector.Misc.Memory.ReadInt64(System.IntPtr)')
  - [ReadIntPtr(address,is64)](#M-MonoNativeInjector-Misc-Memory-ReadIntPtr-System-IntPtr,System-Boolean- 'MonoNativeInjector.Misc.Memory.ReadIntPtr(System.IntPtr,System.Boolean)')
  - [ReadString(address,encoding)](#M-MonoNativeInjector-Misc-Memory-ReadString-System-IntPtr,System-Text-Encoding- 'MonoNativeInjector.Misc.Memory.ReadString(System.IntPtr,System.Text.Encoding)')
  - [WriteBytes(address,buffer)](#M-MonoNativeInjector-Misc-Memory-WriteBytes-System-IntPtr,System-Collections-Generic-IEnumerable{System-Byte}- 'MonoNativeInjector.Misc.Memory.WriteBytes(System.IntPtr,System.Collections.Generic.IEnumerable{System.Byte})')
- [MonoInjector](#T-MonoNativeInjector-Abstractions-MonoInjector 'MonoNativeInjector.Abstractions.MonoInjector')
  - [CreateInjector()](#M-MonoNativeInjector-Abstractions-MonoInjector-CreateInjector 'MonoNativeInjector.Abstractions.MonoInjector.CreateInjector')
  - [CreateInstanceOfInjector()](#M-MonoNativeInjector-Abstractions-MonoInjector-CreateInstanceOfInjector 'MonoNativeInjector.Abstractions.MonoInjector.CreateInstanceOfInjector')
- [MonoInjector32](#T-MonoNativeInjector-MonoInjectors-MonoInjector32 'MonoNativeInjector.MonoInjectors.MonoInjector32')
  - [#ctor()](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-#ctor-System-Diagnostics-Process- 'MonoNativeInjector.MonoInjectors.MonoInjector32.#ctor(System.Diagnostics.Process)')
  - [Eject(assemblyPtr,namespaceName,className,methodName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-Eject-System-IntPtr,System-String,System-String,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector32.Eject(System.IntPtr,System.String,System.String,System.String)')
  - [ExecuteAssemblyMethod(funcAddress,attach,args)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-ExecuteAssemblyMethod-System-IntPtr,System-Boolean,System-IntPtr[]- 'MonoNativeInjector.MonoInjectors.MonoInjector32.ExecuteAssemblyMethod(System.IntPtr,System.Boolean,System.IntPtr[])')
  - [GetMonoAssemblyImage(monoAssembly)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoAssemblyImage-System-IntPtr- 'MonoNativeInjector.MonoInjectors.MonoInjector32.GetMonoAssemblyImage(System.IntPtr)')
  - [GetMonoClass(monoAssemblyImage,nameSpace,className)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoClass-System-IntPtr,System-String,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector32.GetMonoClass(System.IntPtr,System.String,System.String)')
  - [GetMonoFuncAddress(funcName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoFuncAddress-System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector32.GetMonoFuncAddress(System.String)')
  - [GetMonoMethod(monoClass,methodName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoMethod-System-IntPtr,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector32.GetMonoMethod(System.IntPtr,System.String)')
  - [GetRootDomain()](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetRootDomain 'MonoNativeInjector.MonoInjectors.MonoInjector32.GetRootDomain')
  - [Inject(assemblyPath,namespaceName,className,methodName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-Inject-System-String,System-String,System-String,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector32.Inject(System.String,System.String,System.String,System.String)')
  - [InvokeMonoMethod(monoMethod)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-InvokeMonoMethod-System-IntPtr- 'MonoNativeInjector.MonoInjectors.MonoInjector32.InvokeMonoMethod(System.IntPtr)')
  - [OpenMonoAssembly(assemblyPath,status)](#M-MonoNativeInjector-MonoInjectors-MonoInjector32-OpenMonoAssembly-System-String,System-Int32@- 'MonoNativeInjector.MonoInjectors.MonoInjector32.OpenMonoAssembly(System.String,System.Int32@)')
- [MonoInjector32Creator](#T-MonoNativeInjector-MonoInjectorCreators-MonoInjector32Creator 'MonoNativeInjector.MonoInjectorCreators.MonoInjector32Creator')
  - [#ctor()](#M-MonoNativeInjector-MonoInjectorCreators-MonoInjector32Creator-#ctor-System-Diagnostics-Process- 'MonoNativeInjector.MonoInjectorCreators.MonoInjector32Creator.#ctor(System.Diagnostics.Process)')
  - [CreateInjector()](#M-MonoNativeInjector-MonoInjectorCreators-MonoInjector32Creator-CreateInjector 'MonoNativeInjector.MonoInjectorCreators.MonoInjector32Creator.CreateInjector')
- [MonoInjector64](#T-MonoNativeInjector-MonoInjectors-MonoInjector64 'MonoNativeInjector.MonoInjectors.MonoInjector64')
  - [#ctor()](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-#ctor-System-Diagnostics-Process- 'MonoNativeInjector.MonoInjectors.MonoInjector64.#ctor(System.Diagnostics.Process)')
  - [Eject(assemblyPtr,namespaceName,className,methodName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-Eject-System-IntPtr,System-String,System-String,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector64.Eject(System.IntPtr,System.String,System.String,System.String)')
  - [ExecuteAssemblyMethod(funcAddress,attach,args)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-ExecuteAssemblyMethod-System-IntPtr,System-Boolean,System-IntPtr[]- 'MonoNativeInjector.MonoInjectors.MonoInjector64.ExecuteAssemblyMethod(System.IntPtr,System.Boolean,System.IntPtr[])')
  - [GetMonoAssemblyImage(monoAssembly)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoAssemblyImage-System-IntPtr- 'MonoNativeInjector.MonoInjectors.MonoInjector64.GetMonoAssemblyImage(System.IntPtr)')
  - [GetMonoClass(monoAssemblyImage,nameSpace,className)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoClass-System-IntPtr,System-String,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector64.GetMonoClass(System.IntPtr,System.String,System.String)')
  - [GetMonoFuncAddress(funcName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoFuncAddress-System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector64.GetMonoFuncAddress(System.String)')
  - [GetMonoMethod(monoClass,methodName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoMethod-System-IntPtr,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector64.GetMonoMethod(System.IntPtr,System.String)')
  - [GetRootDomain()](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetRootDomain 'MonoNativeInjector.MonoInjectors.MonoInjector64.GetRootDomain')
  - [Inject(assemblyPath,namespaceName,className,methodName)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-Inject-System-String,System-String,System-String,System-String- 'MonoNativeInjector.MonoInjectors.MonoInjector64.Inject(System.String,System.String,System.String,System.String)')
  - [InvokeMonoMethod(monoMethod)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-InvokeMonoMethod-System-IntPtr- 'MonoNativeInjector.MonoInjectors.MonoInjector64.InvokeMonoMethod(System.IntPtr)')
  - [OpenMonoAssembly(assemblyPath,status)](#M-MonoNativeInjector-MonoInjectors-MonoInjector64-OpenMonoAssembly-System-String,System-Int32@- 'MonoNativeInjector.MonoInjectors.MonoInjector64.OpenMonoAssembly(System.String,System.Int32@)')
- [MonoInjector64Creator](#T-MonoNativeInjector-MonoInjectorCreators-MonoInjector64Creator 'MonoNativeInjector.MonoInjectorCreators.MonoInjector64Creator')
  - [#ctor()](#M-MonoNativeInjector-MonoInjectorCreators-MonoInjector64Creator-#ctor-System-Diagnostics-Process- 'MonoNativeInjector.MonoInjectorCreators.MonoInjector64Creator.#ctor(System.Diagnostics.Process)')
  - [CreateInjector()](#M-MonoNativeInjector-MonoInjectorCreators-MonoInjector64Creator-CreateInjector 'MonoNativeInjector.MonoInjectorCreators.MonoInjector64Creator.CreateInjector')
- [MonoInjectorBase](#T-MonoNativeInjector-Abstractions-MonoInjectorBase 'MonoNativeInjector.Abstractions.MonoInjectorBase')
  - [#ctor(gameProcess)](#M-MonoNativeInjector-Abstractions-MonoInjectorBase-#ctor-System-Diagnostics-Process- 'MonoNativeInjector.Abstractions.MonoInjectorBase.#ctor(System.Diagnostics.Process)')
  - [WaitForMonoModule()](#M-MonoNativeInjector-Abstractions-MonoInjectorBase-WaitForMonoModule 'MonoNativeInjector.Abstractions.MonoInjectorBase.WaitForMonoModule')

<a name='T-MonoNativeInjector-Common-IMonoInjector'></a>
## IMonoInjector `type`

##### Namespace

MonoNativeInjector.Common

##### Summary

Defines the contract for Mono injectors, including methods for injecting and ejecting assemblies, and disposing resources.

<a name='M-MonoNativeInjector-Common-IMonoInjector-Eject-System-IntPtr,System-String,System-String,System-String-'></a>
### Eject(assemblyPtr,namespaceName,className,methodName) `method`

##### Summary

Ejects an injected assembly from a Mono process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the injected assembly. |
| namespaceName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace from which the assembly will be ejected. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The class from which the assembly will be ejected. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The method from which the assembly will be ejected. |

<a name='M-MonoNativeInjector-Common-IMonoInjector-Inject-System-String,System-String,System-String,System-String-'></a>
### Inject(assemblyPath,namespaceName,className,methodName) `method`

##### Summary

Injects an assembly into a Mono process.

##### Returns

A pointer indicating the result of the injection process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the assembly to be injected. |
| namespaceName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The target namespace within the process. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The target class within the namespace. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The target method within the class to inject the assembly into. |

<a name='T-MonoNativeInjector-Misc-Logger'></a>
## Logger `type`

##### Namespace

MonoNativeInjector.Misc

##### Summary

Provides logging functionalities for the application, allowing the logging of information, warnings, and debug messages.

<a name='M-MonoNativeInjector-Misc-Logger-LogDebug-System-String-'></a>
### LogDebug(message) `method`

##### Summary

Logs a debug message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The message to log. |

<a name='M-MonoNativeInjector-Misc-Logger-LogInfo-System-String-'></a>
### LogInfo(message) `method`

##### Summary

Logs an informational message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The message to log. |

<a name='M-MonoNativeInjector-Misc-Logger-LogWarning-System-String-'></a>
### LogWarning(message) `method`

##### Summary

Logs a warning message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The message to log. |

<a name='T-MonoNativeInjector-Main'></a>
## Main `type`

##### Namespace

MonoNativeInjector

<a name='M-MonoNativeInjector-Main-CloseMonoInjector-System-IntPtr-'></a>
### CloseMonoInjector(instanceOfMonoInjector) `method`

##### Summary

Closes and disposes of the MonoInjector instance.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| instanceOfMonoInjector | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Handle to the instance of MonoInjector. |

<a name='M-MonoNativeInjector-Main-Eject-System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr-'></a>
### Eject(instanceOfMonoInjector,assemblyPtr,namespaceName,className,methodName) `method`

##### Summary

Ejects an injected assembly from the specified namespace and class method within a Mono process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| instanceOfMonoInjector | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Handle to the instance of MonoInjector. |
| assemblyPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the assembly in memory. |
| namespaceName | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the namespace in memory. |
| className | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the class name in memory. |
| methodName | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the method name in memory. |

<a name='M-MonoNativeInjector-Main-Inject-System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr,System-IntPtr-'></a>
### Inject(instanceOfMonoInjector,assemblyPathPtr,namespacePtr,classNamePtr,methodNamePtr) `method`

##### Summary

Injects an assembly into the specified namespace and class method within a Mono process.

##### Returns

A pointer indicating the result of the injection process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| instanceOfMonoInjector | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Handle to the instance of MonoInjector. |
| assemblyPathPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the assembly path in memory. |
| namespacePtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the namespace in memory. |
| classNamePtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the class name in memory. |
| methodNamePtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the method name in memory. |

<a name='M-MonoNativeInjector-Main-OpenMonoInjector-System-IntPtr-'></a>
### OpenMonoInjector(processNamePtr) `method`

##### Summary

Opens an instance of MonoInjector based on the process name provided. It tries to find the process up to 5 times before failing.

##### Returns

A handle to the instance of MonoInjector.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| processNamePtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | Pointer to the process name in memory. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') | Thrown when an instance of MonoInjector cannot be created. |

<a name='M-MonoNativeInjector-Main-SetLogger-System-IntPtr-'></a>
### SetLogger(logger) `method`

##### Summary

Sets the logger function pointer to be used for logging within the application.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logger | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the managed delegate for logging. |

<a name='T-MonoNativeInjector-Misc-Memory'></a>
## Memory `type`

##### Namespace

MonoNativeInjector.Misc

##### Summary

Manages memory operations (read, write, allocate, free) in a specified process.

<a name='M-MonoNativeInjector-Misc-Memory-#ctor-System-Int32-'></a>
### #ctor(processId) `constructor`

##### Summary

Initializes a new instance of the [Memory](#T-MonoNativeInjector-Misc-Memory 'MonoNativeInjector.Misc.Memory') class for a given process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| processId | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The ID of the process to operate on. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.AccessViolationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.AccessViolationException 'System.AccessViolationException') | Thrown when access to the process is denied. |

<a name='P-MonoNativeInjector-Misc-Memory-ProcessHandle'></a>
### ProcessHandle `property`

##### Summary

Gets the handle to the process being operated on.

<a name='M-MonoNativeInjector-Misc-Memory-AllocateMemory-System-Int32-'></a>
### AllocateMemory(size) `method`

##### Summary

Allocates memory in the target process.

##### Returns

The address of the allocated memory.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| size | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The size of the memory to allocate. |

<a name='M-MonoNativeInjector-Misc-Memory-FreeMemory-System-IntPtr-'></a>
### FreeMemory(address) `method`

##### Summary

Frees previously allocated memory in the target process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The address of the memory to free. |

<a name='M-MonoNativeInjector-Misc-Memory-InvokeRemoteThread-System-IntPtr-'></a>
### InvokeRemoteThread(address) `method`

##### Summary

Invokes a thread at a specified address within the target process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The address of the code to execute in the new thread. |

<a name='M-MonoNativeInjector-Misc-Memory-ReadAndCastToStruct``1-System-IntPtr-'></a>
### ReadAndCastToStruct\`\`1(address) `method`

##### Summary

Reads a structure of type T from a specified memory address.

##### Returns

The structure read from the address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to read from. |

<a name='M-MonoNativeInjector-Misc-Memory-ReadInt16-System-IntPtr-'></a>
### ReadInt16(address) `method`

##### Summary

Reads a 16-bit integer from a specified memory address.

##### Returns

The 16-bit integer read from the address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to read from. |

<a name='M-MonoNativeInjector-Misc-Memory-ReadInt32-System-IntPtr-'></a>
### ReadInt32(address) `method`

##### Summary

Reads a 32-bit integer from a specified memory address.

##### Returns

The 32-bit integer read from the address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to read from. |

<a name='M-MonoNativeInjector-Misc-Memory-ReadInt64-System-IntPtr-'></a>
### ReadInt64(address) `method`

##### Summary

Reads a 64-bit integer from a specified memory address.

##### Returns

The 64-bit integer read from the address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to read from. |

<a name='M-MonoNativeInjector-Misc-Memory-ReadIntPtr-System-IntPtr,System-Boolean-'></a>
### ReadIntPtr(address,is64) `method`

##### Summary

Reads a pointer from a specified memory address, considering the architecture (32/64 bit).

##### Returns

The pointer read from the address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to read from. |
| is64 | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Indicates whether to read as 64-bit pointer. |

<a name='M-MonoNativeInjector-Misc-Memory-ReadString-System-IntPtr,System-Text-Encoding-'></a>
### ReadString(address,encoding) `method`

##### Summary

Reads a string from a specified memory address.

##### Returns

The string read from the address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to read from. |
| encoding | [System.Text.Encoding](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Text.Encoding 'System.Text.Encoding') | The encoding of the string. |

<a name='M-MonoNativeInjector-Misc-Memory-WriteBytes-System-IntPtr,System-Collections-Generic-IEnumerable{System-Byte}-'></a>
### WriteBytes(address,buffer) `method`

##### Summary

Writes bytes to a specified memory address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The memory address to write to. |
| buffer | [System.Collections.Generic.IEnumerable{System.Byte}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.Byte}') | The bytes to write. |

<a name='T-MonoNativeInjector-Abstractions-MonoInjector'></a>
## MonoInjector `type`

##### Namespace

MonoNativeInjector.Abstractions

<a name='M-MonoNativeInjector-Abstractions-MonoInjector-CreateInjector'></a>
### CreateInjector() `method`

##### Summary

When implemented in a derived class, creates and returns a new instance of a class implementing the IMonoInjector interface.

##### Returns

An IMonoInjector instance created by the derived class.

##### Parameters

This method has no parameters.

<a name='M-MonoNativeInjector-Abstractions-MonoInjector-CreateInstanceOfInjector'></a>
### CreateInstanceOfInjector() `method`

##### Summary

Creates and returns an instance of a class implementing the IMonoInjector interface using the abstract CreateInjector method defined in derived classes.

##### Returns

An instance of a class implementing the IMonoInjector interface.

##### Parameters

This method has no parameters.

<a name='T-MonoNativeInjector-MonoInjectors-MonoInjector32'></a>
## MonoInjector32 `type`

##### Namespace

MonoNativeInjector.MonoInjectors

##### Summary

A Mono injector implementation for 32-bit processes.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-#ctor-System-Diagnostics-Process-'></a>
### #ctor() `constructor`

##### Summary

A Mono injector implementation for 32-bit processes.

##### Parameters

This constructor has no parameters.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-Eject-System-IntPtr,System-String,System-String,System-String-'></a>
### Eject(assemblyPtr,namespaceName,className,methodName) `method`

##### Summary

Ejects an assembly from the Mono process.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the previously injected assembly. |
| namespaceName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace of the class. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The class of the method. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The method to invoke for ejection. |

##### Remarks

Currently implements the same logic as injection for demonstration. Actual ejection logic may differ based on the Mono runtime's capabilities.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-ExecuteAssemblyMethod-System-IntPtr,System-Boolean,System-IntPtr[]-'></a>
### ExecuteAssemblyMethod(funcAddress,attach,args) `method`

##### Summary

Executes a Mono assembly method.

##### Returns

The result of the method execution.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| funcAddress | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The function address in the Mono runtime. |
| attach | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Whether to attach the executing thread to the Mono runtime. |
| args | [System.IntPtr[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr[] 'System.IntPtr[]') | Arguments for the function call. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoAssemblyImage-System-IntPtr-'></a>
### GetMonoAssemblyImage(monoAssembly) `method`

##### Summary

Gets the Mono image for an assembly.

##### Returns

A pointer to the Mono image.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoAssembly | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono assembly. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoClass-System-IntPtr,System-String,System-String-'></a>
### GetMonoClass(monoAssemblyImage,nameSpace,className) `method`

##### Summary

Retrieves a Mono class from an assembly image.

##### Returns

A pointer to the Mono class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoAssemblyImage | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono image. |
| nameSpace | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace of the class. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the class. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoFuncAddress-System-String-'></a>
### GetMonoFuncAddress(funcName) `method`

##### Summary

Gets the address of a function in the Mono runtime by name.

##### Returns

The address of the function, or [Zero](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr.Zero 'System.IntPtr.Zero') if not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| funcName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the function. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetMonoMethod-System-IntPtr,System-String-'></a>
### GetMonoMethod(monoClass,methodName) `method`

##### Summary

Retrieves a Mono method from a class.

##### Returns

A pointer to the Mono method.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoClass | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono class. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the method. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-GetRootDomain'></a>
### GetRootDomain() `method`

##### Summary

Retrieves the Mono root domain for the process.

##### Returns

A pointer to the Mono root domain.

##### Parameters

This method has no parameters.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-Inject-System-String,System-String,System-String,System-String-'></a>
### Inject(assemblyPath,namespaceName,className,methodName) `method`

##### Summary

Injects an assembly into the Mono process.

##### Returns

A pointer to the injected assembly.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The file path of the assembly to inject. |
| namespaceName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace containing the class. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The class containing the method to invoke. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The method to invoke upon injection. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | Thrown when the root domain or assembly image cannot be obtained, or class or method lookup fails. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-InvokeMonoMethod-System-IntPtr-'></a>
### InvokeMonoMethod(monoMethod) `method`

##### Summary

Invokes a Mono method.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoMethod | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono method. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector32-OpenMonoAssembly-System-String,System-Int32@-'></a>
### OpenMonoAssembly(assemblyPath,status) `method`

##### Summary

Opens a Mono assembly from the specified path.

##### Returns

A pointer to the opened Mono assembly.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The file path of the assembly. |
| status | [System.Int32@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32@ 'System.Int32@') | The status code returned by the Mono runtime. |

<a name='T-MonoNativeInjector-MonoInjectorCreators-MonoInjector32Creator'></a>
## MonoInjector32Creator `type`

##### Namespace

MonoNativeInjector.MonoInjectorCreators

##### Summary

A factory class for creating instances of [MonoInjector32](#T-MonoNativeInjector-MonoInjectors-MonoInjector32 'MonoNativeInjector.MonoInjectors.MonoInjector32'), tailored for 32-bit Mono processes.

<a name='M-MonoNativeInjector-MonoInjectorCreators-MonoInjector32Creator-#ctor-System-Diagnostics-Process-'></a>
### #ctor() `constructor`

##### Summary

A factory class for creating instances of [MonoInjector32](#T-MonoNativeInjector-MonoInjectors-MonoInjector32 'MonoNativeInjector.MonoInjectors.MonoInjector32'), tailored for 32-bit Mono processes.

##### Parameters

This constructor has no parameters.

<a name='M-MonoNativeInjector-MonoInjectorCreators-MonoInjector32Creator-CreateInjector'></a>
### CreateInjector() `method`

##### Summary

Creates an instance of [MonoInjector32](#T-MonoNativeInjector-MonoInjectors-MonoInjector32 'MonoNativeInjector.MonoInjectors.MonoInjector32'), providing the necessary implementation of [IMonoInjector](#T-MonoNativeInjector-Common-IMonoInjector 'MonoNativeInjector.Common.IMonoInjector') for 32-bit processes.

##### Returns

An instance of [MonoInjector32](#T-MonoNativeInjector-MonoInjectors-MonoInjector32 'MonoNativeInjector.MonoInjectors.MonoInjector32') specialized for injecting into 32-bit Mono processes.

##### Parameters

This method has no parameters.

<a name='T-MonoNativeInjector-MonoInjectors-MonoInjector64'></a>
## MonoInjector64 `type`

##### Namespace

MonoNativeInjector.MonoInjectors

##### Summary

A Mono injector implementation for 64-bit processes.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-#ctor-System-Diagnostics-Process-'></a>
### #ctor() `constructor`

##### Summary

A Mono injector implementation for 64-bit processes.

##### Parameters

This constructor has no parameters.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-Eject-System-IntPtr,System-String,System-String,System-String-'></a>
### Eject(assemblyPtr,namespaceName,className,methodName) `method`

##### Summary

Ejects an assembly from the Mono process, adapted for 64-bit processes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the previously injected assembly. |
| namespaceName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace of the class. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The class of the method. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The method to invoke for ejection. |

##### Remarks

Implements the same logic as injection for demonstration. Actual ejection logic may vary.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-ExecuteAssemblyMethod-System-IntPtr,System-Boolean,System-IntPtr[]-'></a>
### ExecuteAssemblyMethod(funcAddress,attach,args) `method`

##### Summary

Executes a method in the Mono runtime using the specified function address and arguments.

##### Returns

The result of the method execution.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| funcAddress | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The function address in the Mono runtime. |
| attach | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Whether to attach the executing thread to the Mono runtime. |
| args | [System.IntPtr[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr[] 'System.IntPtr[]') | Arguments for the function call. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoAssemblyImage-System-IntPtr-'></a>
### GetMonoAssemblyImage(monoAssembly) `method`

##### Summary

Gets the Mono image for an assembly, which contains metadata and code.

##### Returns

A pointer to the Mono image.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoAssembly | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono assembly. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoClass-System-IntPtr,System-String,System-String-'></a>
### GetMonoClass(monoAssemblyImage,nameSpace,className) `method`

##### Summary

Retrieves a Mono class from an assembly image based on the namespace and class name.

##### Returns

A pointer to the Mono class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoAssemblyImage | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono image. |
| nameSpace | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace of the class. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the class. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoFuncAddress-System-String-'></a>
### GetMonoFuncAddress(funcName) `method`

##### Summary

Resolves the address of a function in the Mono runtime by its name.

##### Returns

The address of the function if found; otherwise, [Zero](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr.Zero 'System.IntPtr.Zero').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| funcName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the function. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetMonoMethod-System-IntPtr,System-String-'></a>
### GetMonoMethod(monoClass,methodName) `method`

##### Summary

Retrieves a Mono method from a class based on the method name.

##### Returns

A pointer to the Mono method.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoClass | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono class. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the method. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-GetRootDomain'></a>
### GetRootDomain() `method`

##### Summary

Retrieves the root domain of the Mono runtime, necessary for various Mono API calls.

##### Returns

A pointer to the Mono root domain.

##### Parameters

This method has no parameters.

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-Inject-System-String,System-String,System-String,System-String-'></a>
### Inject(assemblyPath,namespaceName,className,methodName) `method`

##### Summary

Injects an assembly into the Mono process, specifically designed for 64-bit architectures.

##### Returns

A pointer to the injected assembly.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The file path of the assembly to inject. |
| namespaceName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The namespace containing the class. |
| className | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The class containing the method to invoke. |
| methodName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The method to invoke upon injection. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | Thrown when the root domain, assembly, class, or method cannot be obtained. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-InvokeMonoMethod-System-IntPtr-'></a>
### InvokeMonoMethod(monoMethod) `method`

##### Summary

Invokes a Mono method, executing the specified logic within the Mono runtime.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| monoMethod | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the Mono method. |

<a name='M-MonoNativeInjector-MonoInjectors-MonoInjector64-OpenMonoAssembly-System-String,System-Int32@-'></a>
### OpenMonoAssembly(assemblyPath,status) `method`

##### Summary

Opens a Mono assembly from the specified path, necessary for the injection process.

##### Returns

A pointer to the opened Mono assembly.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblyPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The file path of the assembly. |
| status | [System.Int32@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32@ 'System.Int32@') | The status code returned by the Mono runtime after attempting to open the assembly. |

<a name='T-MonoNativeInjector-MonoInjectorCreators-MonoInjector64Creator'></a>
## MonoInjector64Creator `type`

##### Namespace

MonoNativeInjector.MonoInjectorCreators

##### Summary

A factory class for creating instances of [MonoInjector64](#T-MonoNativeInjector-MonoInjectors-MonoInjector64 'MonoNativeInjector.MonoInjectors.MonoInjector64'), specifically designed for 64-bit Mono processes.

<a name='M-MonoNativeInjector-MonoInjectorCreators-MonoInjector64Creator-#ctor-System-Diagnostics-Process-'></a>
### #ctor() `constructor`

##### Summary

A factory class for creating instances of [MonoInjector64](#T-MonoNativeInjector-MonoInjectors-MonoInjector64 'MonoNativeInjector.MonoInjectors.MonoInjector64'), specifically designed for 64-bit Mono processes.

##### Parameters

This constructor has no parameters.

<a name='M-MonoNativeInjector-MonoInjectorCreators-MonoInjector64Creator-CreateInjector'></a>
### CreateInjector() `method`

##### Summary

Creates an instance of [MonoInjector64](#T-MonoNativeInjector-MonoInjectors-MonoInjector64 'MonoNativeInjector.MonoInjectors.MonoInjector64'), providing a concrete implementation of [IMonoInjector](#T-MonoNativeInjector-Common-IMonoInjector 'MonoNativeInjector.Common.IMonoInjector') optimized for 64-bit processes.

##### Returns

An instance of [MonoInjector64](#T-MonoNativeInjector-MonoInjectors-MonoInjector64 'MonoNativeInjector.MonoInjectors.MonoInjector64') that is specialized for injecting assemblies into 64-bit Mono processes.

##### Parameters

This method has no parameters.

<a name='T-MonoNativeInjector-Abstractions-MonoInjectorBase'></a>
## MonoInjectorBase `type`

##### Namespace

MonoNativeInjector.Abstractions

##### Summary

Provides a base implementation for Mono injectors, handling common setup and disposal functionalities.

<a name='M-MonoNativeInjector-Abstractions-MonoInjectorBase-#ctor-System-Diagnostics-Process-'></a>
### #ctor(gameProcess) `constructor`

##### Summary

Initializes a new instance of the [MonoInjectorBase](#T-MonoNativeInjector-Abstractions-MonoInjectorBase 'MonoNativeInjector.Abstractions.MonoInjectorBase') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| gameProcess | [System.Diagnostics.Process](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Diagnostics.Process 'System.Diagnostics.Process') | The target game process for injection. |

<a name='M-MonoNativeInjector-Abstractions-MonoInjectorBase-WaitForMonoModule'></a>
### WaitForMonoModule() `method`

##### Summary

Waits for the Mono module to be loaded into the game process, essential for the injection process.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | Thrown if the Mono module cannot be found after several attempts. |
