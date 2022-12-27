using OSStateDotNet.Services;

namespace OSStateDotNet
{
    public interface IOsState
    {
        ISystemSleepStateService CurrentService { get; }
    }
}