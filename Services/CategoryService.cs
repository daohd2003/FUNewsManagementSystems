using BusinessObjects;
using DataAccess;
using FUDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>("Category");
        }

        public async Task<CategoryDTO> GetCategoryById(short id)
        {
#pragma warning disable CS8603
            return await _httpClient.GetFromJsonAsync<CategoryDTO>($"Category/{id}");
#pragma warning restore CS8603
        }

        public async Task Create(CategoryDTO category)
        {
            await _httpClient.PostAsJsonAsync("Category", category);
        }

        public async Task Update(CategoryDTO category)
        {
            await _httpClient.PutAsJsonAsync($"Category/{category.CategoryId}", category);
        }

        public async Task Delete(short id)
        {
            await _httpClient.DeleteAsync($"Category/{id}");
        }
    }
}
