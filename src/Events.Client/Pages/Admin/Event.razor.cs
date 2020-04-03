using Events.Client.Services.Api;
using Events.Shared.Formatters;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace Events.Client.Pages.Admin
{
    public class EventBase : ComponentBase
    {
        [Inject]
        protected EventApiService EventApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int CategoryId { get; set; }
        [Parameter]
        public int Id { get; set; }

        public bool IsLoading;
        protected string Name { get; set; }
        protected EventModel _event = new EventModel();

        protected async Task HandleSubmit()
        {
            if (_event.Id == 0)
            {
                await EventApiService.Post(_event);
            }
            else
            {
                await EventApiService.Put(_event.Id, _event);
            }

            NavigationManager.NavigateTo($"admin/category/{_event.CategoryId}");
        }

        protected async Task HandleDelete()
        {
            await EventApiService.Delete(Id);
            NavigationManager.NavigateTo($"admin/category/{_event.CategoryId}");
        }

        protected async Task HandleUnlock()
        {
            await EventApiService.Unlock(Id);
            await Load();
        }

        protected async Task HandleLock()
        {
            await EventApiService.Lock(Id);
            await Load();
        }

        protected async Task Load()
        {
            _event = await EventApiService.Get(Id);
            IsLoading = false;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Id > 0)
                {
                    await Load();
                    Name = _event.Date.ToString(DateTimeFormatter.LongFormat);
                }
                else
                {
                    _event.CategoryId = CategoryId;
                    _event.Date = DateTimeOffset.Now.Date;
                    Name = "New event";
                }

                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            IsLoading = Id > 0;
            NavigationManager.LocationChanged += LocationChanged;
            base.OnInitialized();
        }

        protected async void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            await Load();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
