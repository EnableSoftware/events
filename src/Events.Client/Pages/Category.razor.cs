using Events.Client.Services.Api;
using Events.Client.State;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Client.Pages
{
    public class CategoryBase : ComponentBase
    {
        [Inject]
        protected CategoryApiService CategoryApiService { get; set; }
        [Inject]
        protected EventApiService EventApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected UserState UserState { get; set; }

        [Parameter]
        public int Id { get; set; }

        public bool IsLoading = true;
        protected string Name { get; set; }
        protected IEnumerable<EventModel> UpcomingEvents { get; set; }

        protected HashSet<int> SignUpsLoading { get; set; } = new HashSet<int>();

        protected IEnumerable<EventModel> ConfirmedUpcomingEvents
        {
            get
            {
                return UpcomingEvents.Where(o => o.LockedDate.HasValue).Where(o => o.Attendees.Any(q => q.Id == UserState.UserInfo.Id));
            }
        }

        protected async Task Load()
        {
            var category = await CategoryApiService.Get(Id);
            Name = category.Name;
            UpcomingEvents = await EventApiService.GetUpcomingForCategory(Id);
            IsLoading = false;
            StateHasChanged();
        }

        protected async override Task OnParametersSetAsync()
        {
            await Load();
        }

        public async Task SignUp(EventModel eventModel)
        {
            if (eventModel == null)
            {
                throw new ArgumentNullException(nameof(eventModel));
            }

            SignUpsLoading.Add(eventModel.Id);
            StateHasChanged();
            await EventApiService.SignUp(eventModel.Id);
            eventModel.SignedUpCount++;
            eventModel.SignedUp = true;
            SignUpsLoading.Remove(eventModel.Id);
            StateHasChanged();
        }

        public async Task Cancel(EventModel eventModel, bool reloadRequired = false)
        {
            if (eventModel == null)
            {
                throw new ArgumentNullException(nameof(eventModel));
            }

            SignUpsLoading.Add(eventModel.Id);
            StateHasChanged();
            await EventApiService.SignUp(eventModel.Id);
            if (reloadRequired)
            {
                SignUpsLoading.Remove(eventModel.Id);
                await Load();
            }
            else
            {
                eventModel.SignedUp = false;
                eventModel.SignedUpCount--;
                SignUpsLoading.Remove(eventModel.Id);
                StateHasChanged();
            }
        }
    }
}
