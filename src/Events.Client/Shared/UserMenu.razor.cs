using Events.Client.Services.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Threading.Tasks;

namespace Events.Client.Shared
{
    public class UserMenuBase : ComponentBase
    {
        [Inject] UserApiService UserApiService { get; set; }
        [Inject] SignOutSessionStateManager SignOutManager { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        public Uri ProfileImageUrl { get; protected set; }

        private bool _expandNavMenu;

        protected string NavMenuCssClass => _expandNavMenu ? "expanded" : null;

        public void ToggleMenu()
        {
            _expandNavMenu = !_expandNavMenu;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ProfileImageUrl = await UserApiService.GetProfileImageUrl();
                StateHasChanged();
            }
        }

        protected async Task BeginLogout()
        {
            await SignOutManager.SetSignOutState();
            NavigationManager.NavigateTo("authentication/logout");
        }
    }
}
