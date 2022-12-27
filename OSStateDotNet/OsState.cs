using System.Runtime.InteropServices;
using OSStateDotNet.Services;
using OSStateDotNet.Services.Implementations;

namespace OSStateDotNet
{
    internal sealed class OsState : IOsState
    {
        public OsState()
        {
            CheckOsTypeAndCreateService();
        }

        public ISystemSleepStateService CurrentService { get; private set; }

        private void CheckOsTypeAndCreateService()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                CurrentService = new WindowsSleepStateServiceImpl();

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                CurrentService = new LinuxSleepStateServiceImpl();
        }
    }
}