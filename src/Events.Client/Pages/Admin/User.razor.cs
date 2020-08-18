using Events.Client.Services.Api;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Events.Client.Pages.Admin
{
    public class UserBase : ComponentBase
    {
        [Inject]
        protected UserApiService UserApiService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        public bool IsLoading;
        protected string Name { get; set; }
        protected UserModel _user = new UserModel();

        protected async Task HandleSubmit()
        {
            if (_user.Id == 0)
            {
                await UserApiService.Post(_user);
            }
            else
            {
                await UserApiService.Put(_user.Id, _user);
            }

            NavigationManager.NavigateTo("admin/users");
        }

        protected async Task HandleDelete()
        {
            await UserApiService.Delete(Id);
            NavigationManager.NavigateTo("admin/users");
        }

        protected async Task Load()
        {
            _user = await UserApiService.Get(Id);
            IsLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Id > 0)
                {
                    await Load();
                    Name = _user.Name;
                }
                else
                {
                    Name = "New user";
                }

                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            IsLoading = Id > 0;
            base.OnInitialized();
        }
    }
}
