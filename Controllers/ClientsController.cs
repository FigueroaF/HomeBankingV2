using HomeBankingV1.DTOS;
using HomeBankingV1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace HomeBankingV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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
        [HttpGet("{id}") ]
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
    }
}
