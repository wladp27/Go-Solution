using GoWeb.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Service.State
{
    public class NewEventState
    {
        private EventDTO _unsavedNewEvent = new();
        public void SaveEvent(EventDTO newEvent)=> _unsavedNewEvent = newEvent;
        public EventDTO GetEvent() => _unsavedNewEvent;
        public void ClearEvent() => _unsavedNewEvent = new();

    }
}
