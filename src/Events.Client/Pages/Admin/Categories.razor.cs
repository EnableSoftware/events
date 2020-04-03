using Events.Client.Services.Api;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Client.Pages.Admin
{
    public class CategoriesBase : ComponentBase
    {
        [Inject]
        protected CategoryApiService CategoryApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public bool IsLoading;

        public IEnumerable<CategoryModel> Items { get; set; }

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
            Items = await CategoryApiService.Get();
            IsLoading = false;
        }

        protected void OnRowClick(CategoryModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            NavigationManager.NavigateTo($"admin/category/{item.Id}");
        }
    }
}
