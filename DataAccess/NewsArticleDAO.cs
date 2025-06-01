using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class NewsArticleDAO : SingletonBase<NewsArticleDAO>
    {
        public async Task<List<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles.Include(n => n.Tags).Include(n => n.Category).Include(n => n.CreatedBy).ToListAsync();
        }

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)           // Load thông tin Category
                .Include(n => n.CreatedBy)          // Load thông tin người tạo
                .Include(n => n.UpdatedBy)          // Load thông tin người cập nhật
                .Include(n => n.Tags)               // Load danh sách tags
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task AddAsync(NewsArticle entity)
        {
            _context.NewsArticles.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle entity)
        {
            var existing = await _context.NewsArticles
                .FirstOrDefaultAsync(x => x.NewsArticleId == entity.NewsArticleId);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _context.NewsArticles.FindAsync(id);
            if (entity != null)
            {
                _context.NewsArticles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NewsArticle>> GetByAuthorAsync(short authorId)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedBy.AccountId == authorId)
                .Include(n => n.Tags)
                .Include(n => n.Category)
                .ToListAsync();
        }
    }
}
