namespace MonoNativeInjector.Enums;

[Flags]
internal enum MemoryProtection : uint
{
    Execute = 0x10,
    ExecuteRead = 0x20,
    ExecuteReadWrite = 0x40,
    ExecuteWriteCopy = 0x80,
    NoAccess = 0x1,
    ReadOnly = 0x2,
    ReadWrite = 0x4,
    WriteCopy = 0x8,
    GuardModifierflag = 0x100,
    NoCacheModifierflag = 0x200,
    WriteCombineModifierflag = 0x400
}