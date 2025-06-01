using FUDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISystemAccountService
    {
        Task<IEnumerable<SystemAccountDTO>> GetAccounts();
        Task<SystemAccountDTO> GetAccountById(short id);
        Task Create(SystemAccountDTO account);
        Task Update(SystemAccountDTO account);
        Task Delete(short id);
        Task<LoginResponseDTO?> Login(LoginRequestDTO loginDto);
    }
}
