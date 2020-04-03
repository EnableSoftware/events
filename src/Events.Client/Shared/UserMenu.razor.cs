using Events.Client.Services.Api;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Events.Client.Shared
{
    public class UserMenuBase : ComponentBase
    {
        [Inject] UserApiService UserApiService { get; set; }

        public Uri ProfileImageUrl { get; protected set; }

        private bool _expandNavMenu = false;

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
    }
}
