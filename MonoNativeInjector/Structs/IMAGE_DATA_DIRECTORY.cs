using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_DATA_DIRECTORY
{
    [FieldOffset(0x4)]
    internal uint Size;
}