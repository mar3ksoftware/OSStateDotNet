using System;
using OSStateDotNet.Enums;
using OSStateDotNet.Events;

namespace OSStateDotNet.Services
{
    internal abstract class SystemSleepStateService : ISystemSleepStateService
    {
        protected SystemSleepStateService()
        {
            CurrentState = SystemSleepState.Unknown;
        }

        public SystemSleepState CurrentState { get; private set; }
        public event EventHandler<SystemSleepStateChangedEventArgs> StateChanged;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void OnSystemSleepStateChanged(SystemSleepState newState)
        {
            var evtArgs = new SystemSleepStateChangedEventArgs(newState);
            CurrentState = newState;
            StateChanged?.Invoke(this, evtArgs);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (StateChanged == null) return;
            foreach (var handler in StateChanged.GetInvocationList())
                StateChanged -= (EventHandler<SystemSleepStateChangedEventArgs>) handler;
        }
    }
}