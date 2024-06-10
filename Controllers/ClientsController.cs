using HomeBankingV1.DTOS;
using HomeBankingV1.Models;
using HomeBankingV1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HomeBankingV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IAccountRepository _accountRepository;
        public ClientsController(IClientRepository clientRepository, ICardRepository cardRepository, IAccountRepository accountRepository)
        {
            _clientRepository = clientRepository;
            _cardRepository = cardRepository;
            _accountRepository = accountRepository;
        }


        //[HttpGet]
        //public IActionResult Hello() 
        //{
        //    return Ok("Hello World");
        //}

        [HttpGet]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = clients.Select(c => new ClientDTO(c)).ToList();
                return Ok(clientsDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetClientById(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO(client);

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateAccount()
        {
            try
            {
                // Obtener el email del cliente autenticado
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Forbid();
                }


                // Obtener el cliente por email
                var client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                // Obtener las cuentas del cliente
                var accounts = _accountRepository.GetAccountsByClient(client.Id).ToList();

                // Verificar si el cliente ya tiene 3 cuentas
                if (accounts.Count >= 3)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "El cliente ya tiene 3 cuentas registradas.");
                }

                string accountNumber;
                do
                {
                    accountNumber = "VIN-" + RandomNumberGenerator.GetInt32(0, 99999999);
                } while (_accountRepository.FindAllAccounts().Any(a => a.Number == accountNumber));


                // Crear la nueva cuenta
                var newAccount = new Account
                {
                    Number = accountNumber,
                    Balance = 0,
                    ClientId = client.Id,
                    CreationDate = DateTime.Now,
                };


                // Guardar la cuenta
                _accountRepository.Save(newAccount);

                return StatusCode(StatusCodes.Status201Created, "Cuenta creada");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            // Método para generar un número de cuenta único
            //private string GenerateUniqueAccountNumber()
            //{
            //    string accountNumber;
            //    do
            //    {
            //        accountNumber = "VIN-" + new Random().Next(10000000, 99999999).ToString();
            //    } while (_accountRepository.FindAllAccounts().Any(a => a.Number == accountNumber));

            //    return accountNumber;
            //}
        }


        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                //validamos datos antes
                if (String.IsNullOrEmpty(client.Email) ||
                    String.IsNullOrEmpty(client.Password) ||
                    String.IsNullOrEmpty(client.FirstName) ||
                    String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "Dato Invalidos");

                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(client.Email);
                if (user != null)
                {
                    return StatusCode(403, "Email esta en uso");
                }

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient);
                return Created("", newClient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }//crear tarjeta 
        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateCard([FromBody] CardCreationDTO cardDTO)
        {
            var client = GetAuthenticatedClient();
            if (client == null)
            {
                return Forbid();
            }

            //var cards = _cardRepository.GetCardsByClient(client.Id).ToList();
            //if (cards.Count >= 6)
            //{
            //    return StatusCode(StatusCodes.Status403Forbidden, "El cliente ya tiene 6 tarjetas registradas");
            //}
            var cards = _cardRepository.GetCardsByClient(client.Id).ToList();
            var debitCardsCount = cards.Count(c => c.Type == "debit");
            var creditCardsCount = cards.Count(c => c.Type == "credit");

            if (cardDTO.Type == "debit" && debitCardsCount >= 3)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "El cliente ya tiene el máximo permitido de tarjetas de débito.");
            }

            if (cardDTO.Type == "credit" && creditCardsCount >= 3)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "El cliente ya tiene el máximo permitido de tarjetas de crédito.");
            }

            string cardNumber;
            do
            {
                cardNumber = GenerateUniqueCardNumber();
            } while (!_cardRepository.IsCardNumberUnique(cardNumber));

            var cvv = GenerateCVV();

            var expirationDate = DateTime.Now.AddYears(5);

            var newCard = new Card
            {
                Number = cardNumber,
                Type = cardDTO.Type,
                CardHolder = client.FirstName +" "+ client.LastName,
                Color = cardDTO.Color,
                Cvv = cvv,
                ThruDate = expirationDate,
                ClientId = client.Id
            };

            _cardRepository.Save(newCard);

            return StatusCode(StatusCodes.Status201Created, "Tarjeta creada");
        }

        private Client GetAuthenticatedClient()
        {
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
            if (email == string.Empty)
                throw new UnauthorizedAccessException("Unauthorized");

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
                throw new UnauthorizedAccessException("Unauthorized");

            return client;
        }

        private string GenerateUniqueCardNumber()
        {
            var random = new Random();
            var number = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                number.Append(random.Next(1000, 9999).ToString());
                if (i < 3)
                {
                    number.Append("-");
                }
            }

            return number.ToString();
        }

        private int GenerateCVV()
        {
            var random = new Random();
            var cvv = random.Next(100, 999);
            return cvv;
        }

        [HttpGet("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCards()
        {
            try
            {
                // Obtener el email del cliente autenticado
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Forbid();
                }


                // Obtener el cliente por email
                var client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                // Obtener las cuentas del cliente
                var accounts = _accountRepository.GetAccountsByClient(client.Id).ToList();

                // Verificar si el cliente ya tiene 3 cuentas
                if (accounts.Count >= 3)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "El cliente ya tiene 3 cuentas registradas.");
                }

                string accountNumber;
                do
                {
                    accountNumber = "VIN-" + RandomNumberGenerator.GetInt32(0, 99999999);
                } while (_accountRepository.FindAllAccounts().Any(a => a.Number == accountNumber));


                // Crear la nueva cuenta
                var newAccount = new Account
                {
                    Number = accountNumber,
                    Balance = 0,
                    ClientId = client.Id,
                    CreationDate = DateTime.Now,
                };


                // Guardar la cuenta
                _accountRepository.Save(newAccount);

                return StatusCode(StatusCodes.Status201Created, "Cuenta creada");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}       
