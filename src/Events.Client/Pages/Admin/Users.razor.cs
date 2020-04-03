using Events.Client.Services.Api;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Client.Pages.Admin
{
    public class UsersBase : ComponentBase
    {
        [Inject]
        protected UserApiService UserApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public bool IsLoading;

        public IEnumerable<UserModel> Items { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Load();
                StateHasChanged();
            }
        }

        protected async Task Load()
        {
            Items = await UserApiService.Get();
            IsLoading = false;
        }

        protected void OnRowClick(UserModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            NavigationManager.NavigateTo($"admin/user/{item.Id}");
        }
    }
}
