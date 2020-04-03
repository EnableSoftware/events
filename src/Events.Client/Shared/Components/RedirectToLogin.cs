using Microsoft.AspNetCore.Components;

namespace Events.Client.Shared.Components
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
