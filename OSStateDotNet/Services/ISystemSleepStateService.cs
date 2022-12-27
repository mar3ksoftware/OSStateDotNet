using System;
using OSStateDotNet.Enums;
using OSStateDotNet.Events;

namespace OSStateDotNet.Services
{
    public interface ISystemSleepStateService : IDisposable
    {
        SystemSleepState CurrentState { get; }
        event EventHandler<SystemSleepStateChangedEventArgs> StateChanged;
    }
}