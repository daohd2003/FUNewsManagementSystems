using FUDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly HttpClient _httpClient;

        public SystemAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SystemAccountDTO>> GetAccounts()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SystemAccountDTO>>("SystemAccount");
        }

        public async Task<SystemAccountDTO> GetAccountById(short id)
        {
#pragma warning disable CS8603
            return await _httpClient.GetFromJsonAsync<SystemAccountDTO>($"SystemAccount/{id}");
#pragma warning restore CS8603
        }

        public async Task Create(SystemAccountDTO account)
        {
            await _httpClient.PostAsJsonAsync("SystemAccount", account);
        }

        public async Task Update(SystemAccountDTO account)
        {
            await _httpClient.PutAsJsonAsync($"SystemAccount/{account.AccountId}", account);
        }

        public async Task Delete(short id)
        {
            await _httpClient.DeleteAsync($"SystemAccount/{id}");
        }

        public async Task<LoginResponseDTO?> Login(LoginRequestDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            }
            return null;
        }
    }
}
