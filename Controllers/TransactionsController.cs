using HomeBankingV1.DTOS;
using HomeBankingV1.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using HomeBankingV1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.Xml;

namespace HomeBankingV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }


        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult Transfer([FromBody] TransferDTO transferDTO)
        {
            //verificamos que los parametros no esten vacios
            if (string.IsNullOrEmpty(transferDTO.FromAccountNumber) ||
                string.IsNullOrEmpty(transferDTO.ToAccountNumber) ||
                transferDTO.Amount <= 0 ||
                string.IsNullOrEmpty(transferDTO.Description))
            {
                return StatusCode(400, "Parámetros de entrada no válidos.");
            }


            //verificamos que los numeros de cuenta no sean iguales
            if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber) 
            {
                return StatusCode(400, "Los numeros de cuenta y de destino no pueden ser iguales");
            }

            //verificamos la cuenta de origen 
            Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
            if (fromAccount == null) 
            {
                return StatusCode(404, "Cuenta de origen no encontrada.");
            }

            //verificamos que la cuenta de origen pertenezca al cliente autenticado
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                return Forbid("No se pudo obtener el email del cliente autenticado.");
            }

            //verificamos que la cuenta de destino exista 
            Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
            if (toAccount == null) 
            {
                return StatusCode(404, "Cuenta de destino inexistente.");
            }

            //verificamos que la cuenta de origen tenga el monto disponible 
            if (fromAccount.Balance < transferDTO.Amount) 
            {
                return StatusCode(400, "Fondos insuficientes en la cuenta de origen.");
            }

            //creamos las transacciones DEBIT y CREDIT
             var debitTransaction = new Transaction
            {
                Amount = -transferDTO.Amount,
                Description = transferDTO.Description,
                Date = DateTime.Now,
                Type = "DEBIT",
                AccountId = fromAccount.Id,
            };

            var creditTransaction = new Transaction
            {
                Amount = transferDTO.Amount,
                Description = transferDTO.Description,
                Date = DateTime.Now,
                Type = "CREDIT",
                AccountId = toAccount.Id,
            };

            // Actualizar saldos
            fromAccount.Balance -= transferDTO.Amount;
            toAccount.Balance += transferDTO.Amount;


            //guardamos las transacciones 
            _transactionRepository.Save(debitTransaction);
            _transactionRepository.Save(creditTransaction);

            // guardamos las cuentas actualizadas
            _accountRepository.Save(fromAccount);
            _accountRepository.Save(toAccount);

            return StatusCode(200, "Transferencia exitosa");
        }

        //obtenemos las cuentas a las que queremos transferir  
        [HttpGet]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetTransferAccounts(string clientEmail) 
        {
            try
            {

            
            //obtenemos el cliente por email
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                return Forbid("No se pudo obtener el email del cliente autenticado.");
            }

            // Obtener el cliente por email
            var client = _clientRepository.FindByEmail(email);
            if (client == null)
            {
                return NotFound("Cliente no encontrado");
            }

            // Obtener las cuentas del cliente
            var accounts = _accountRepository.GetAccountsByClient(client.Id).ToList();

            //retornamos las cuentas disponibles para la transferencia
            var transferAccountsDTO = accounts.Select(a => new AccountDTO (a)).ToList();
                //{
                //Id = a.Id,
                //Number = a.Number,
                //Balance = a.Balance,
                //}).ToList();

                return Ok(transferAccountsDTO);
            }
            catch (Exception e) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }
}
