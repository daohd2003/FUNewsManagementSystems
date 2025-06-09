using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllNews();
        Task<NewsArticle> GetNewsById(string id);
        Task Add(NewsArticle news, List<int> tagIds);
        Task Update(NewsArticle news, List<int> newTagIds);
        Task Delete(string id);
        Task<IEnumerable<NewsArticle>> GetByAuthor(short authorId);
    }
}
