using HomeBankingV1.DTOS;
using HomeBankingV1.Models;
using HomeBankingV1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly ITransactionRepository _transactionRepository;

        public LoansController (IClientRepository clientRepository, IAccountRepository accountRepository,
            ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository,
            ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult ApplyForLoan([FromBody] LoanApplicationDTO loanAppDTO)
        {
            try
            {
                //verificamos que el usuario este autenticado  
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized("Usuario no Autenticado");
                }

                // Buscamos el cliente por su email para obtener su ID
                var client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }

                //verificamos que el prestamo exista
                var loan = _loanRepository.FindById(loanAppDTO.LoanId);
                if (loan == null)
                {
                    return Forbid("Prestamo no encontrado");
                }

                //verificamos que el monto no sea 0 y que no sobrepase el maximo autorizado 
                if (loanAppDTO.Amount <= 0 || loanAppDTO.Amount > loan.MaxAmount)
                {
                    return Forbid("Monto invalido");
                }

                //verificamos que los pagos no lleguen vacios y esten dentro del rango permitido
                //if (loanAppDTO.Payments <= 0 || loanAppDTO.Payments < loan.Payments || loanAppDTO.Payments > loan.Payments)
                if (string.IsNullOrEmpty(loanAppDTO.Payments) ||
                    !int.TryParse(loanAppDTO.Payments, out int paymentCount) ||
                    paymentCount <= 0)
                {
                    return Forbid("Cantidad de cuotas invalida"); //REVISAR
                }

                //verificamos que la cuenta de destino exista 
                var account = _accountRepository.FindByNumber(loanAppDTO.ToAccountNumber);
                if (account == null)
                {
                    return Forbid("Cuenta de destino no encontrada");
                }

                //creamos la solicitud de prestamo con un 20% mas del monto 
                var AmountInterest = loanAppDTO.Amount * 1.20;
                var clientLoan = new ClientLoan
                {
                    LoanId = loan.Id,
                    ClientId = client.Id,
                    Amount = AmountInterest,
                    Payments = loanAppDTO.Payments,
                };
                _clientLoanRepository.Save(clientLoan);

                var creditTransaction = new Transaction
                {
                    AccountId = account.Id,
                    Amount = loanAppDTO.Amount,
                    Description = "Loan: " + loan.Name + " to Account " + account.Number,
                    Date = DateTime.Now,
                    Type = "CREDIT",
                };

                //actualizamos el balance de la cuenta sumando el monto del prestamo
                account.Balance += AmountInterest;
                _accountRepository.Save(account);

                //guardamos las transacciones
                _transactionRepository.Save(creditTransaction);

                return StatusCode(StatusCodes.Status201Created, "Prestamo aprobado y Cuenta actualizada");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")] //REVISAR
        public IActionResult GetAvailableLoans(long id)
        {
            try
            {
                var loans = _loanRepository.FindById(id); // Obtenemos todos los préstamos disponibles desde el repositorio de préstamos
                return Ok(loans); // Devolvemos la lista de préstamos como respuesta HTTP 200 OK
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message); // Manejamos cualquier error y devolvemos un StatusCode 500 en caso de que falle
            }
        }

        [HttpGet] //REVISAR
        public IActionResult GetAllLoans()
        {
            try
            {
                var loans = _loanRepository.GetAll(); // Obtenemos todos los préstamos disponibles desde el repositorio de préstamos
                var loansDTO = loans.Select(l => new LoanDTO(l)).ToList();
                return Ok(loans); // Devolvemos la lista de préstamos como respuesta HTTP 200 OK
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message); // Manejamos cualquier error y devolvemos un StatusCode 500 en caso de que falle
            }
        }
    }

}
