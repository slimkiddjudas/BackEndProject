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
                    bool roleExists1 = await _roleManager.RoleExistsAsync(_config["Roles:Admin"]);
                    bool roleExists2 = await _roleManager.RoleExistsAsync(_config["Roles:User"]);

                    if (!roleExists1)
                    {
                        IdentityRole role1 = new IdentityRole(_config["Roles:Admin"]);
                        role1.NormalizedName = _config["Roles:Admin"];

                        await _roleManager.CreateAsync(role1);
                    }
                    if (!roleExists2)
                    {
                        IdentityRole role2 = new IdentityRole(_config["Roles:User"]);
                        role2.NormalizedName = _config["Roles:User"];

                       await  _roleManager.CreateAsync(role2);
                    }

                    await _userManager.AddToRoleAsync(user, _config["Roles:Admin"]);
                    await _userManager.AddToRoleAsync(user, _config["Roles:User"]);

                    return Ok(result);
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = $"An error occured while user creating. {result.Errors.FirstOrDefault().Description}";
                    return Ok(responseModel);
                }

            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;

                return BadRequest(responseModel);
            }
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            RegisterResponseModel registerResponseModel = new RegisterResponseModel();
            Console.WriteLine("1");

            try
            {
                if (!ModelState.IsValid)
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = "Try Again.";

                    return BadRequest(registerResponseModel);
                }

                User existsUser = await _userManager.FindByEmailAsync(model.Email);
                Console.WriteLine("2");

                if (existsUser != null)
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = "This user already exists.";

                    return BadRequest(registerResponseModel);
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

                Console.WriteLine("3");

                var result = await _userManager.CreateAsync(user, model.Password.Trim());

                Console.WriteLine("4");

                if (result.Succeeded)
                {
                    bool roleExists = await _roleManager.RoleExistsAsync(_config["Roles:User"]);

                    Console.WriteLine("5");

                    if (!roleExists)
                    {
                        IdentityRole role = new IdentityRole(_config["Roles:User"]);
                        role.NormalizedName = _config["Roles:User"];

                        await _roleManager.CreateAsync(role);
                        Console.WriteLine("6");
                    }

                    await _userManager.AddToRoleAsync(user, _config["Roles:User"]);
                    Console.WriteLine("7");

                    User currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                    Console.WriteLine("8");
                    AccessTokenGenerator accessTokenGenerator = new AccessTokenGenerator(_context, _config, currentUser);
                    Console.WriteLine("9");
                    ApplicationUserTokens userTokens = await accessTokenGenerator.GetToken();
                    Console.WriteLine("10");

                    //var authRoles = from role in _context.Roles
                    //                join userRole in _context.UserRoles
                    //                on role.Id equals userRole.RoleId
                    //                where userRole.UserId == user.Id
                    //                select new { RoleName = role.Name };

                    //foreach (var item in authRoles)
                    //{
                    //    registerResponseModel.Roles.Add(item.RoleName);
                    //};

                    var authRoles = await _userManager.GetRolesAsync(currentUser);

                    Console.WriteLine("11");

                    registerResponseModel.Email = user.Email;
                    registerResponseModel.UserName = user.UserName;
                    registerResponseModel.UserId = user.Id;
                    registerResponseModel.Token = new Token()
                    {
                        TokenBody = userTokens.Value,
                        ExpireDate = userTokens.ExpireDate,
                        RefreshToken = currentUser.RefreshToken,
                        RefreshTokenExpireDate = currentUser.RefreshTokenExpireDate
                    };
                    Console.WriteLine("13");
                    registerResponseModel.FirstName = user.FirstName;
                    registerResponseModel.LastName = user.LastName;
                    registerResponseModel.Message = "User is created successfully";
                    registerResponseModel.Status = true;
                    registerResponseModel.Roles = authRoles.ToList();
                    Console.WriteLine("14");


                    return Ok(registerResponseModel);
                }
                else
                {
                    registerResponseModel.Status = false;
                    registerResponseModel.Message = $"An error occured while user creating. {result.Errors.FirstOrDefault().Description}";
                    return Ok(registerResponseModel);
                }
                
            }
            catch (Exception ex)
            {
                registerResponseModel.Status = false;
                registerResponseModel.Message = ex.Message;

                return BadRequest(registerResponseModel);
            }
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
            User currentUser = _userManager.Users.Where(u => u.RefreshToken == token).ToList()[0];
            var accessTokenExpireDate = new DateTime(
                currentUser.RefreshTokenExpireDate.Value.Year, 
                currentUser.RefreshTokenExpireDate.Value.Month, 
                currentUser.RefreshTokenExpireDate.Value.Day, 
                currentUser.RefreshTokenExpireDate.Value.Hour, 
                currentUser.RefreshTokenExpireDate.Value.Minute - 5, 
                currentUser.RefreshTokenExpireDate.Value.Second);


            if (accessTokenExpireDate < DateTime.Now  && currentUser.RefreshTokenExpireDate > DateTime.Now)
            {
                AccessTokenGenerator accessTokenGenerator = new AccessTokenGenerator(_context, _config, currentUser);
                ApplicationUserTokens userTokens = await accessTokenGenerator.GetToken();

                return Ok(new Token()
                {
                    TokenBody = userTokens.Value,
                    ExpireDate = userTokens.ExpireDate,
                    RefreshToken = currentUser.RefreshToken,
                    RefreshTokenExpireDate = currentUser.RefreshTokenExpireDate
                });
            }
            else
            {
                return Unauthorized("Token already useable.");
            }
        }
        [HttpGet("authMe")]
        public async Task<IActionResult> AuthMe([FromHeader] string token)
        {
            var result = _authorizationService.AuthMe(token);

            return Ok(result);
        }
    }
}
