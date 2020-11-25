using Events.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            return await _httpClient.GetFromJsonAsync<IEnumerable<EventModel>>("api/event");
        }

        public async Task<EventModel> Get(int id)
        {
            return await _httpClient.GetFromJsonAsync<EventModel>($"api/event/{id}");
        }

        public async Task<IEnumerable<EventModel>> GetUpcomingForCategory(int id)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EventModel>>($"api/event/get-for-category/{id}");
        }

        public async Task<int> Post(EventModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/event", model);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task Put(int id, EventModel model)
        {
            await _httpClient.PutAsJsonAsync($"api/event/{id}", model);
        }

        public async Task Delete(int id)
        {
            var uri = new Uri($"api/event/{id}");
            await _httpClient.DeleteAsync(uri);
        }

        public async Task SignUp(int id)
        {
            await _httpClient.PutAsJsonAsync($"api/event/sign-up/{id}", true);
        }

        public async Task CancelSignUp(int id)
        {
            await _httpClient.PutAsJsonAsync($"api/event/sign-up/{id}", false);
        }

        public async Task Lock(int id)
        {
            var uri = new Uri($"api/event/lock-event/{id}");
            await _httpClient.PutAsync(uri, null);
        }

        public async Task Unlock(int id)
        {
            var uri = new Uri($"api/event/unlock-event/{id}");
            await _httpClient.PutAsync(uri, null);
        }
    }
}
