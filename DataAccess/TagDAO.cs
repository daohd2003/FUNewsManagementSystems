using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TagDAO : SingletonBase<TagDAO>
    {
        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag entity)
        {
            _context.Tags.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Tags.FindAsync(id);
            if (entity != null)
            {
                _context.Tags.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
