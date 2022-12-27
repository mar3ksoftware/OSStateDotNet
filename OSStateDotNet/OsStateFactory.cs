namespace OSStateDotNet
{
    public sealed class OsStateFactory
    {
        private static OsStateFactory _factory;
        private IOsState _stateService;

        public static OsStateFactory Factory => _factory ??= new OsStateFactory();

        public IOsState StateService
        {
            get
            {
                CheckAndInitializeStateService();
                return _stateService;
            }
        }

        private void CheckAndInitializeStateService()
        {
            if (_stateService != null) return;
            _stateService = new OsState();
        }
    }
}