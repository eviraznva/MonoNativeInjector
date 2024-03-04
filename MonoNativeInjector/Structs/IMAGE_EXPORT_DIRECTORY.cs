using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_EXPORT_DIRECTORY
{
    [FieldOffset(0x18)]
    internal uint NumberOfNames; 
    [FieldOffset(0x1C)]
    internal uint AddressOfFunctions; 
    [FieldOffset(0x20)]
    internal uint AddressOfNames; 
    [FieldOffset(0x24)]
    internal uint AddressOfNameOrdinals; 
}