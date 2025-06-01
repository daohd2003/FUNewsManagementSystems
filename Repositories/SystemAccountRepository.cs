using BusinessObjects;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public async Task Add(SystemAccount account)
        {
            await SystemAccountDAO.Instance.AddAsync(account);
        }

        public async Task Delete(short id)
        {
            await SystemAccountDAO.Instance.DeleteAsync(id);
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccounts()
        {
            return await SystemAccountDAO.Instance.GetAllAsync();
        }

        public async Task<SystemAccount> GetAccountById(short id)
        {
            return await SystemAccountDAO.Instance.GetByIdAsync(id);
        }

        public async Task Update(SystemAccount account)
        {
            await SystemAccountDAO.Instance.UpdateAsync(account);
        }

        public async Task<SystemAccount?> GetByEmail(string email)
        {
            return await SystemAccountDAO.Instance.GetByEmail(email);
        }

        public async Task<SystemAccount?> Login(string email, string password)
        {
            return await SystemAccountDAO.Instance.Login(email, password);
        }

        public async Task<bool> HasNewsArticle(int categoryId)
        {
            return await SystemAccountDAO.Instance.HasNewsArticle(categoryId);
        }

    }
}
