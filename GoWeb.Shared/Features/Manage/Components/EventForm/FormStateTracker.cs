using GoWeb.Shared.Model;
using GoWeb.Shared.Service.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Components.EventForm
{
    public class FormStateTracker:ComponentBase, IDisposable
    {
        [Inject]
        public AppState AppState { get; set; } = default!;

        [CascadingParameter]
        private EditContext CascadedEditContext { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (CascadedEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(FormStateTracker)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(FormStateTracker)} " +
                    $"inside an {nameof(EditForm)}.");
            }
            CascadedEditContext.OnFieldChanged += HandleFieldChanged;
        }

        private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            var ev =(EventDTO)e.FieldIdentifier.Model;
            if(ev.Id==0)
            {
                AppState.NewEventState.SaveEvent(ev);
            }
        }

        public void Dispose()
        {
            if (CascadedEditContext != null)
            {
                CascadedEditContext.OnFieldChanged -= HandleFieldChanged;
            }
        }

    }
}
