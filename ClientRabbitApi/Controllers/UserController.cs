using ClientLibrary.Repository.Interaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientRabbitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        [Route("User/GetAll")]
        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }
    }
}
