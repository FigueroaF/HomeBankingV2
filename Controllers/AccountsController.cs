using HomeBankingV1.DTOS;
using HomeBankingV1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountRepository.FindAllAccounts();
                var accountsDTO = accounts.Select(c => new AccountDTO(c)).ToList();
                return Ok(accountsDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{Id}")]
        public IActionResult GetAccountsById(long id)
        {
            try
            {
                var accounts = _accountRepository.FindAccountById(id);
                var accountsDTO = new AccountDTO(accounts);
                return Ok(accountsDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
