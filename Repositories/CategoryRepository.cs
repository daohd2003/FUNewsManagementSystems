using BusinessObjects;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task Add(Category category)
        {
            await CategoryDAO.Instance.AddAsync(category);
        }

        public async Task Delete(short id)
        {
            await CategoryDAO.Instance.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await CategoryDAO.Instance.GetAllAsync();
        }

        public async Task<Category> GetCategoryById(short id)
        {
            return await CategoryDAO.Instance.GetByIdAsync(id);
        }

        public async Task Update(Category category)
        {
            await CategoryDAO.Instance.UpdateAsync(category);
        }

        public async Task<bool> HasNewsArticle(short categoryId)
        {
            return await CategoryDAO.Instance.HasNewsArticle(categoryId);
        }
    }
}
