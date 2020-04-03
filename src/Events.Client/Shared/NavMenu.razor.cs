using Microsoft.AspNetCore.Components;
using System;

namespace Events.Client.Shared
{
    public class NavMenuBase : ComponentBase
    {
        [Parameter]
        public Action ToggleClicked { get; set; }

        protected void NavMenuToggle()
        {
            ToggleClicked();
        }
    }
}
