using System.Text;
using System.ComponentModel;
using MonoNativeInjector.Enums;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MonoNativeInjector.Misc;

/// <summary>
/// Manages memory operations (read, write, allocate, free) in a specified process.
/// </summary>
internal class Memory : IDisposable
{
    private readonly List<IntPtr> allocatedMemory = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Memory"/> class for a given process.
    /// </summary>
    /// <param name="processId">The ID of the process to operate on.</param>
    /// <exception cref="AccessViolationException">Thrown when access to the process is denied.</exception>
    public Memory(int processId)
    {
        ProcessHandle = WindowsNative.OpenProcess(ProcessAccessFlags.AllAccess, false, processId);

        if (ProcessHandle == IntPtr.Zero)
            throw new AccessViolationException();
        
        Logger.LogDebug($"Opened process with ID {processId}");
    }
    
    /// <summary>
    /// Reads a 32-bit integer from a specified memory address.
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <returns>The 32-bit integer read from the address.</returns>
    public int ReadInt32(IntPtr address) => Read(address, out int value) ? value : 0;
    
    /// <summary>
    /// Reads a 16-bit integer from a specified memory address.
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <returns>The 16-bit integer read from the address.</returns>
    public short ReadInt16(IntPtr address) => Read(address, out short value) ? value : (short)0;
    
    /// <summary>
    /// Reads a 64-bit integer from a specified memory address.
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <returns>The 64-bit integer read from the address.</returns>
    public long ReadInt64(IntPtr address) => Read(address, out long value) ? value : 0;
    
    /// <summary>
    /// Reads a pointer from a specified memory address, considering the architecture (32/64 bit).
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <param name="is64">Indicates whether to read as 64-bit pointer.</param>
    /// <returns>The pointer read from the address.</returns>
    public IntPtr ReadIntPtr(IntPtr address, bool is64 = true) => (IntPtr)(is64 ? ReadInt64(address) : ReadInt32(address));
    
    /// <summary>
    /// Reads a string from a specified memory address.
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <param name="encoding">The encoding of the string.</param>
    /// <returns>The string read from the address.</returns>
    public string ReadString(IntPtr address, Encoding encoding)
    {
        var buffer = new byte[1024];
        
        if (!WindowsNative.ReadProcessMemory(ProcessHandle, address, buffer, buffer.Length, out _))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        Logger.LogDebug($"Read string from 0x{address.ToInt64():X}: {encoding.GetString(buffer.TakeWhile(b => b != 0).ToArray())}");
        
        return encoding.GetString(buffer.TakeWhile(b => b != 0).ToArray());
    }

    /// <summary>
    /// Reads a structure of type T from a specified memory address.
    /// </summary>
    /// <param name="address">The memory address to read from.</param>
    /// <returns>The structure read from the address.</returns>
    public T ReadAndCastToStruct<T>(IntPtr address) where T : unmanaged => Read(address, out T value) ? value : default;
    
    /// <summary>
    /// Writes bytes to a specified memory address.
    /// </summary>
    /// <param name="address">The memory address to write to.</param>
    /// <param name="buffer">The bytes to write.</param>
    public void WriteBytes(IntPtr address, IEnumerable<byte> buffer)
    {
        var bufferArray = buffer.ToArray();
        
        if (!WindowsNative.WriteProcessMemory(ProcessHandle, address, bufferArray, bufferArray.Length, out _))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        Logger.LogDebug($"Wrote {bufferArray.Length} bytes to 0x{address.ToInt64():X}");
    }
    
    /// <summary>
    /// Allocates memory in the target process.
    /// </summary>
    /// <param name="size">The size of the memory to allocate.</param>
    /// <returns>The address of the allocated memory.</returns>
    public IntPtr AllocateMemory(int size)
    {
        var address = WindowsNative.VirtualAllocEx(ProcessHandle, IntPtr.Zero, size,
            AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);
        
        if (address == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        allocatedMemory.Add(address);
        
        Logger.LogDebug($"Memory has been allocated at 0x{address.ToInt64():X} with size {size}");
        
        return address;
    }
    
    /// <summary>
    /// Invokes a thread at a specified address within the target process.
    /// </summary>
    /// <param name="address">The address of the code to execute in the new thread.</param>
    public void InvokeRemoteThread(IntPtr address)
    {
        var threadHandle = WindowsNative.CreateRemoteThread(ProcessHandle, IntPtr.Zero, 0, address, IntPtr.Zero, ThreadCreationFlags.None, out _);
        
        if (threadHandle == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        var result = WindowsNative.WaitForSingleObject(threadHandle, -1);
        
        if (result == WaitResult.WAIT_FAILED)
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        Logger.LogDebug($"Thread has been executed at 0x{address.ToInt64():X} with result {result}");
        
        WindowsNative.CloseHandle(threadHandle);
    }
    
    /// <summary>
    /// Frees previously allocated memory in the target process.
    /// </summary>
    /// <param name="address">The address of the memory to free.</param>
    public void FreeMemory(IntPtr address)
    {
        var indexOfAllocatedMemoryInfo = allocatedMemory.FindIndex(am => am == address);
        
        if (indexOfAllocatedMemoryInfo == -1) return;
        
        var allocatedMemoryInfo = allocatedMemory[indexOfAllocatedMemoryInfo];
        
        if (!WindowsNative.VirtualFreeEx(ProcessHandle, allocatedMemoryInfo, 0, AllocationType.Release))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        Logger.LogDebug($"Memory has been freed at 0x{allocatedMemoryInfo.ToInt64():X}");
        
        allocatedMemory.RemoveAt(indexOfAllocatedMemoryInfo);
    }
    
    public void Dispose()
    {
        ReadOnlySpan<IntPtr> allocatedMemorySpan = allocatedMemory.ToArray();
        
        foreach (var am in allocatedMemorySpan) FreeMemory(am);
        
        _ = WindowsNative.CloseHandle(ProcessHandle);
        
        Logger.LogDebug("Process handle has been closed");
    }

    private bool Read<T>(IntPtr address, out T outVal) where T : unmanaged
    {
        outVal = default!;
        
        var buffer = new byte[Unsafe.SizeOf<T>()];
        
        if (!WindowsNative.ReadProcessMemory(ProcessHandle, address, buffer, buffer.Length, out _))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        
        outVal = Unsafe.As<byte, T>(ref buffer[0]);
        
        Logger.LogDebug($"Read {typeof(T).Name} from 0x{address.ToInt64():X}: {outVal}");
        
        return true;
    }
    
    /// <summary>
    /// Gets the handle to the process being operated on.
    /// </summary>
    public IntPtr ProcessHandle { get; }
}