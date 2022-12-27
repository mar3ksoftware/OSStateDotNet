using System;
using OSStateDotNet.Enums;

namespace OSStateDotNet.Events
{
    public sealed class SystemSleepStateChangedEventArgs : EventArgs
    {
        internal SystemSleepStateChangedEventArgs(SystemSleepState state)
        {
            State = state;
        }

        public SystemSleepState State { get; }
    }
}