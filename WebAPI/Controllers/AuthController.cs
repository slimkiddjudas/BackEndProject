using Business.Abstract;
using Business.Security;
using Business.Security.Abstract;
using Business.Security.Models;
using DataAccess.Concrete.EntityFramework;
using Entity.Concrete;
using Entity.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContextDb _context;
        private readonly IConfiguration _config;

        private IAuthorizationService _authorizationService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ContextDb context, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = context;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var result = _userManager.Users;
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _context.Users.Where(u => u.Id == id).ToList()[0];
            var result = await _userManager.DeleteAsync(user);
            return Ok(result);
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var result = await _authorizationService.RegisterAdmin(model);

            return Ok(result);
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var result = await _authorizationService.RegisterUser(model);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authorizationService.Login(model);

            return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
        {
            var result = _authorizationService.RefreshToken(token);

            return Ok(result);
        }

        [HttpGet("authMe")]
        public async Task<IActionResult> AuthMe([FromHeader] string token)
        {
            var result = _authorizationService.AuthMe(token);

            return Ok(result);
        }
    }
}
