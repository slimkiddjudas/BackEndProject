using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public AuthController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var result = _userManager.Users;
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> CreateUser(User user)
        {
            var result = await _userManager.CreateAsync(user,"UmutCerk31698=D");
            return Ok(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return Ok(result);
        }

    }
}
