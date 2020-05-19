using Events.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Events.Client.Services.Api
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoryModel>> Get()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryModel>>("api/category");
        }

        public async Task<CategoryModel> Get(int id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryModel>($"api/category/{id}");
        }

        public async Task<int> Post(CategoryModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/category", model);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task Put(int id, CategoryModel model)
        {
            await _httpClient.PutAsJsonAsync($"api/category/{id}", model);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/category/{id}");
        }
    }
}
