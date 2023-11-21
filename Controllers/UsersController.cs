using Microsoft.AspNetCore.Mvc;
using Ski_Service_Backend.Dto;
using Ski_Service_Backend.Model;
using Ski_Service_Backend.Services;

namespace Ski_Service_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public UsersController(AppDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto Model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == Model.UserName);
            if (user == null)

            {
                return BadRequest("User nicht vorhanden");

            }
            if (user.Password.Equals(Model.Password)) 
            {
                var token = _tokenService.CreateToken(Model.UserName);
                return Ok(
                    new JsonResult(new { token = token, username = user.UserName })
                );

            }

            return BadRequest("invalid login data");

        }
    }
}
