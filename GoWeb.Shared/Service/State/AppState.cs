using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Service.State
{
    public class AppState
    {
        public NewEventState NewEventState { get; } 

        public AppState()
        {
            NewEventState = new NewEventState();
        }

    }
}
