using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_DOS_HEADER
{
    [FieldOffset(0x3C)]
    internal uint e_lfanew;
}