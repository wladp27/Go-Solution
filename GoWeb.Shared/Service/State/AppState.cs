using GoWeb.Shared.Interfaces;

namespace GoWeb.Shared.Service.State
{
    public class AppState
    {
        public NewEventState NewEventState { get; } 
        public readonly ISelectCityState selectCityState;
        private bool isInitialized = false;


        public AppState(ISelectCityState selectCityState)
        {
            NewEventState = new NewEventState();
            this.selectCityState = selectCityState;
        }

        public async Task Initialize()
        {
            await selectCityState.Initialize();
            isInitialized = true;
        }

    }
}
