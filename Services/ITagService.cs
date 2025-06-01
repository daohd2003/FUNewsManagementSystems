using DataAccess;
using FUDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDTO>> GetTags();
        Task<TagDTO> GetTagById(int id);
        Task Create(TagDTO tag);
        Task Update(TagDTO tag);
        Task Delete(int id);
    }
}
