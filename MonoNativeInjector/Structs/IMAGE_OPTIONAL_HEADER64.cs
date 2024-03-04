using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_OPTIONAL_HEADER64
{
    [FieldOffset(0x6C)]
    internal IMAGE_DATA_DIRECTORY imageDataExportDirectory;
}