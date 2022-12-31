using Business.Security;
using Business.Security.Models;
using DataAccess.Concrete.EntityFramework;
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
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContextDb _context;
        private readonly IConfiguration _config;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ContextDb context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = context;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var result = _userManager.Users;
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    responseModel.Status = false;
                    responseModel.Message = "Try Again.";

                    return BadRequest(responseModel);
                }

                User existsUser = await _userManager.FindByEmailAsync(model.Email);

                if (existsUser != null)
                {
                    responseModel.Status = false;
                    responseModel.Message = "This user already exists.";

                    return BadRequest(responseModel);
                }

                User user = new User();

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.EmailConfirmed = true;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumberConfirmed = false;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = true;
                user.AccessFailedCount = 0;

                var result = await _userManager.CreateAsync(user, model.Password.Trim());

                if (result.Succeeded)
                {
                    bool roleExists = await _roleManager.RoleExistsAsync(_config["Roles:User"]);

                    if (!roleExists)
                    {
                        IdentityRole role = new IdentityRole(_config["Roles:User"]);
                        role.NormalizedName = _config["Roles:User"];

                        _roleManager.CreateAsync(role).Wait();
                    }

                    _userManager.AddToRoleAsync(user, _config["Roles:User"]).Wait();
                    responseModel.Status = true;
                    responseModel.Message = "User has been created successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = $"An error occured while user creating. {result.Errors.FirstOrDefault().Description}";
                }
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;

                return BadRequest(responseModel);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                if (ModelState.IsValid == false)
                {
                    responseModel.Status = false;
                    responseModel.Message = "Try Again.";
                    return BadRequest(responseModel);
                }

                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    responseModel.Status = false;
                    responseModel.Message = "Try Again.";
                    return Unauthorized(responseModel);
                }

                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (signInResult.Succeeded == false)
                {
                    responseModel.Status = false;
                    responseModel.Message = "User name or password is incorrect.";

                    return Unauthorized(responseModel);
                }

                User currentUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
                AccessTokenGenerator accessTokenGenerator = new AccessTokenGenerator(_context, _config, currentUser);

                ApplicationUserTokens userTokens = accessTokenGenerator.GetToken();

                responseModel.Status = true;
                responseModel.Message = "User is Logged in.";
                responseModel.Token = new Token()
                {
                    TokenBody = userTokens.Value,
                    ExpireDate = userTokens.ExpireDate
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;

                return BadRequest(responseModel);
            }
        }

    }
}
