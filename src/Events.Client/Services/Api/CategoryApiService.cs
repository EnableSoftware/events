using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
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
            return await _httpClient.GetJsonAsync<IEnumerable<CategoryModel>>("api/category");
        }

        public async Task<CategoryModel> Get(int id)
        {
            return await _httpClient.GetJsonAsync<CategoryModel>($"api/category/{id}");
        }

        public async Task<int> Post(CategoryModel model)
        {
            return await _httpClient.PostJsonAsync<int>("api/category", model);
        }

        public async Task Put(int id, CategoryModel model)
        {
            await _httpClient.PutJsonAsync($"api/category/{id}", model);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/category/{id}");
        }
    }
}
