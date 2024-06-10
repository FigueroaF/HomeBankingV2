using HomeBankingV1.DTOS;
using HomeBankingV1.Models;
using HomeBankingV1.Repositories;
using HomeBankingV1.Repositories.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
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
        // Método para generar un número de cuenta único
        private string GenerateUniqueAccountNumber()
        {
            string accountNumber;
            do
            {
                accountNumber = "VIN-" + new Random().Next(10000000, 99999999).ToString();
            } while (_accountRepository.FindAllAccounts().Any(a => a.Number == accountNumber));

            return accountNumber;
        }
    }
}
