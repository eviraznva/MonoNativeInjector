using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_NT_HEADERS
{
    [FieldOffset(0x16)]
    internal IMAGE_OPTIONAL_HEADER32 OptionalHeader;
}