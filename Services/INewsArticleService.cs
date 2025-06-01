using DataAccess;
using FUDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticleDTO>> GetNewsArticles();
        Task<NewsArticleDTO> GetNewsArticleById(string id);
        Task Create(NewsArticleDTO newsArticle);
        Task Update(NewsArticleDTO newsArticle);
        Task Delete(string id);
        Task<IEnumerable<NewsArticleDTO>> GetNewsArticlesByAuthor();
    }
}
