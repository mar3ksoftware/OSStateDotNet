namespace OSStateDotNet.Enums
{
    public enum SystemSleepState : byte
    {
        Unknown = 0x00,
        ChangingState = 0x01,
        Suspended = 0x02,
        Resumed = 0x03
    }
}