using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_NT_HEADERS64
{
    [FieldOffset(0x18)]
    internal IMAGE_OPTIONAL_HEADER64 OptionalHeader;
}