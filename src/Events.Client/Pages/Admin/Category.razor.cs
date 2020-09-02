using Events.Client.Services.Api;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Client.Pages.Admin
{
    public class CategoryBase : ComponentBase
    {
        [Inject]
        protected CategoryApiService CategoryApiService { get; set; }
        [Inject]
        protected EventApiService EventApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected bool IsLoading;
        protected string Name { get; set; }
        protected CategoryModel Category = new CategoryModel();
        protected IEnumerable<EventModel> Events { get; set; }

        protected async Task HandleSubmit()
        {
            if (Category.Id == 0)
            {
                await CategoryApiService.Post(Category);
            }
            else
            {
                await CategoryApiService.Put(Category.Id, Category);
            }

            NavigationManager.NavigateTo("admin/categories");
        }

        protected async Task HandleDelete()
        {
            await CategoryApiService.Delete(Id);
            NavigationManager.NavigateTo("admin/categories");
        }

        protected async Task Load()
        {
            Category = await CategoryApiService.Get(Id);
            Events = await EventApiService.GetUpcomingForCategory(Id);
            IsLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Id > 0)
                {
                    await Load();
                    Name = Category.Name;
                }
                else
                {
                    Name = "New category";
                }

                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            IsLoading = Id > 0;
            base.OnInitialized();
        }

        protected void OnRowClick(EventModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            NavigationManager.NavigateTo($"admin/event/{Id}/{item.Id}");
        }
    }
}
