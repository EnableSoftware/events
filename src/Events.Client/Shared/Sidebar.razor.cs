using Events.Client.Services.Api;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Client.Shared
{
    public class SidebarBase : ComponentBase
    {
        [Inject]
        protected CategoryApiService CategoryApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private bool _expandNavMenu = false;
        protected bool IsLoading = true;
        protected IEnumerable<CategoryModel> Categories { get; set; }

        protected string NavMenuCssClass => _expandNavMenu ? "expanded" : null;

        public async void ToggleSidebar()
        {
            _expandNavMenu = !_expandNavMenu;

            if (_expandNavMenu)
            {
                IsLoading = true;
                StateHasChanged();

                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (authState.User.Identity != null && authState.User.Identity.IsAuthenticated)
                {
                    Categories = await CategoryApiService.Get();
                    IsLoading = false;
                }
            }

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.LocationChanged += LocationChanged;
            base.OnInitialized();
        }

        protected void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            _expandNavMenu = false;
            StateHasChanged();
        }
    }
}
