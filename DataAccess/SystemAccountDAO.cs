using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SystemAccountDAO : SingletonBase<SystemAccountDAO>
    {
        public async Task<List<SystemAccount>> GetAllAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task<SystemAccount?> GetByIdAsync(short id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }

        public async Task AddAsync(SystemAccount entity)
        {
            _context.SystemAccounts.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SystemAccount entity)
        {
            var existingEntity = await _context.SystemAccounts.FindAsync(entity.AccountId);
            if (existingEntity == null)
            {
                throw new Exception("Entity not found");
            }

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(short id)
        {
            var entity = await _context.SystemAccounts.FindAsync(id);
            if (entity != null)
            {
                _context.SystemAccounts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SystemAccount?> GetByEmail(string email)
        {
            using var _context = new FunewsManagementContext();
            return await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> Login(string email, string password)
        {
            using var _context = new FunewsManagementContext();
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public async Task<bool> HasNewsArticle(int accountId)
        {
            using var _context = new FunewsManagementContext();
            return await _context.NewsArticles.AnyAsync(n => n.UpdatedById == accountId || n.CreatedById == accountId);
        }
    }
}
