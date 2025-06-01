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
    public class NewsArticleService : INewsArticleService
    {
        private readonly HttpClient _httpClient;

        public NewsArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetNewsArticles()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticleDTO>>("NewsArticle");
        }

        public async Task<NewsArticleDTO> GetNewsArticleById(string id)
        {
#pragma warning disable CS8603
            return await _httpClient.GetFromJsonAsync<NewsArticleDTO>($"NewsArticle/{id}");
#pragma warning restore CS8603
        }

        public async Task Create(NewsArticleDTO newsArticle)
        {
            await _httpClient.PostAsJsonAsync("NewsArticle", newsArticle);
        }

        public async Task Update(NewsArticleDTO newsArticle)
        {
            await _httpClient.PutAsJsonAsync($"NewsArticle/{newsArticle.NewsArticleId}", newsArticle);
        }

        public async Task Delete(string id)
        {
            await _httpClient.DeleteAsync($"NewsArticle/{id}");
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetNewsArticlesByAuthor()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticleDTO>>($"NewsArticle/MyArticles");
        }
    }
}
