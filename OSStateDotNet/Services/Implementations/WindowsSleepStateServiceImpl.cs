using System;
using Microsoft.Win32;
using OSStateDotNet.Enums;

namespace OSStateDotNet.Services.Implementations
{
    internal sealed class WindowsSleepStateServiceImpl : SystemSleepStateService
    {
        public WindowsSleepStateServiceImpl()
        {
            SystemEvents.PowerModeChanged += SystemEventsOnPowerModeChanged;
        }

        private void SystemEventsOnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    OnSystemSleepStateChanged(SystemSleepState.Resumed);
                    break;
                case PowerModes.StatusChange:
                    OnSystemSleepStateChanged(SystemSleepState.ChangingState);
                    break;
                case PowerModes.Suspend:
                    OnSystemSleepStateChanged(SystemSleepState.Suspended);
                    break;
                default:
                    OnSystemSleepStateChanged(SystemSleepState.Unknown);
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SystemEvents.PowerModeChanged -= SystemEventsOnPowerModeChanged;
        }
    }
}