using System.Runtime.InteropServices;

namespace MonoNativeInjector.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct IMAGE_OPTIONAL_HEADER32
{
    [FieldOffset(0x68)]
    internal IMAGE_DATA_DIRECTORY imageDataExportDirectory;
}