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
        public async Task Add(NewsArticle news, List<int> tagIds)
        {
            await NewsArticleDAO.Instance.AddAsync(news, tagIds);
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

        public async Task Update(NewsArticle news, List<int> newTagIds)
        {
            await NewsArticleDAO.Instance.UpdateAsync(news, newTagIds);
        }
        public async Task<IEnumerable<NewsArticle>> GetByAuthor(short authorId)
        {
            return await NewsArticleDAO.Instance.GetByAuthorAsync(authorId);
        }

    }
}
