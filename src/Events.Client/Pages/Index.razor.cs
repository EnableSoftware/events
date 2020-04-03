using Events.Client.Services.Api;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        protected CategoryApiService CategoryApiService { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected bool IsLoading;
        protected IEnumerable<CategoryModel> Categories { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsLoading = true;
                await Load();
            }
        }

        protected async Task Load()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity != null && authState.User.Identity.IsAuthenticated)
            {
                Categories = await CategoryApiService.Get();
            }

            IsLoading = false;
            StateHasChanged();
        }
    }
}
