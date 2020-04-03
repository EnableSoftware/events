using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Events.Client.Services.Api
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        public EventApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<EventModel>> Get()
        {
            return await _httpClient.GetJsonAsync<IEnumerable<EventModel>>("api/event");
        }

        public async Task<EventModel> Get(int id)
        {
            return await _httpClient.GetJsonAsync<EventModel>($"api/event/{id}");
        }

        public async Task<IEnumerable<EventModel>> GetUpcomingForCategory(int id)
        {
            return await _httpClient.GetJsonAsync<IEnumerable<EventModel>>($"api/event/get-for-category/{id}");
        }

        public async Task<int> Post(EventModel model)
        {
            return await _httpClient.PostJsonAsync<int>("api/event", model);
        }

        public async Task Put(int id, EventModel model)
        {
            await _httpClient.PutJsonAsync($"api/event/{id}", model);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/event/{id}");
        }

        public async Task SignUp(int id)
        {
            await _httpClient.PutJsonAsync($"api/event/sign-up/{id}", true);
        }

        public async Task CancelSignUp(int id)
        {
            await _httpClient.PutJsonAsync($"api/event/sign-up/{id}", false);
        }

        public async Task Lock(int id)
        {
            await _httpClient.PutJsonAsync($"api/event/lock-event/{id}", null);
        }

        public async Task Unlock(int id)
        {
            await _httpClient.PutJsonAsync($"api/event/unlock-event/{id}", null);
        }
    }
}
