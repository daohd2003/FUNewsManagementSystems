using BusinessObjects;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TagRepository : ITagRepository
    {
        public async Task Add(Tag tag)
        {
            await TagDAO.Instance.AddAsync(tag);
        }

        public async Task Delete(int id)
        {
            await TagDAO.Instance.DeleteAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            return await TagDAO.Instance.GetAllAsync();
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await TagDAO.Instance.GetByIdAsync(id);
        }

        public async Task Update(Tag tag)
        {
            await TagDAO.Instance.UpdateAsync(tag);
        }
    }
}
