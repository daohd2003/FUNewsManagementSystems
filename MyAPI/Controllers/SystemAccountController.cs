using AutoMapper;
using BusinessObjects;
using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IMapper _mapper;

        public SystemAccountController(ISystemAccountRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _repo.GetAllAccounts();
            var result = _mapper.Map<IEnumerable<SystemAccountDTO>>(accounts);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _repo.GetAccountById(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SystemAccount dto)
        {
            await _repo.Add(dto);
            return Content("Insert success!");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(short id, SystemAccount dto)
        {
            if (id != dto.AccountId) return BadRequest();

            var existing = await _repo.GetAccountById(id);
            if (existing == null) return NotFound();

            await _repo.Update(dto);
            return Content("Update success!");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(short id)
        {
            var account = await _repo.GetAccountById(id);
            if (account == null) return NotFound();

            bool hasNews = await _repo.HasNewsArticle(id);
            if (hasNews) return BadRequest("Cannot delete this account because it has created news articles.");

            await _repo.Delete(id);
            return Content("Delete success!");
        }
    }
}
