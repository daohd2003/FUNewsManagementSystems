using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO : SingletonBase<CategoryDAO>
    {

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.ParentCategory).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(object id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            var local = _context.Categories.Local.FirstOrDefault(c => c.CategoryId == entity.CategoryId);

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity != null)
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasNewsArticle(short categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
        }
    }
}
