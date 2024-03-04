namespace MonoNativeInjector.Enums;

internal enum WaitResult: uint 
{
    WAIT_ABANDONED = 0x80,
    WAIT_OBJECT_0 = 0x0,
    WAIT_TIMEOUT = 0x102,
    WAIT_FAILED = 0xFFFFFFFF
}