namespace MonoNativeInjector.Enums;

[Flags]
internal enum ThreadCreationFlags
{
    None = 0x0,
    CREATE_SUSPENDED = 0x4,
    STACK_SIZE_PARAM_IS_A_RESERVATION = 0x10000
}