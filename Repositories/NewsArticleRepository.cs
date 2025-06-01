using BusinessObjects;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public async Task Add(NewsArticle news)
        {
            await NewsArticleDAO.Instance.AddAsync(news);
        }

        public async Task Delete(string id)
        {
            await NewsArticleDAO.Instance.DeleteAsync(id);
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNews()
        {
            return await NewsArticleDAO.Instance.GetAllAsync();
        }

        public async Task<NewsArticle> GetNewsById(string id)
        {
            return await NewsArticleDAO.Instance.GetByIdAsync(id);
        }

        public async Task Update(NewsArticle news)
        {
            await NewsArticleDAO.Instance.UpdateAsync(news);
        }
        public async Task<IEnumerable<NewsArticle>> GetByAuthor(short authorId)
        {
            return await NewsArticleDAO.Instance.GetByAuthorAsync(authorId);
        }

    }
}
