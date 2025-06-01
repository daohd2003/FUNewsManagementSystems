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
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;

        public TagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TagDTO>> GetTags()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TagDTO>>("Tag");
        }

        public async Task<TagDTO> GetTagById(int id)
        {
#pragma warning disable CS8603
            return await _httpClient.GetFromJsonAsync<TagDTO>($"Tag/{id}");
#pragma warning restore CS8603
        }

        public async Task Create(TagDTO tag)
        {
            await _httpClient.PostAsJsonAsync("Tag", tag);
        }

        public async Task Update(TagDTO tag)
        {
            await _httpClient.PutAsJsonAsync($"Tag/{tag.TagId}", tag);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"Tag/{id}");
        }
    }
}
