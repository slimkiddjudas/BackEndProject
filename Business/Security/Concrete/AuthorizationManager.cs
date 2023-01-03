using Business.Security.Abstract;
using Business.Security.Models;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Concrete
{
    public class AuthorizationManager : IAuthorizationService
    {
        private readonly ContextDb _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthorizationManager(ContextDb context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
        }

        public async Task<IDataResult<LoginResponseModel>> Login(LoginModel loginModel)
        {
            LoginResponseModel loginResponseModel = new LoginResponseModel();

            try
            {
                //if (ModelState.IsValid == false)
                //{
                //    loginResponseModel.Status = false;
                //    loginResponseModel.Message = "Try Again.";
                //    return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
                //}

                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user == null)
                {
                    loginResponseModel.Status = false;
                    loginResponseModel.Message = "Try Again.";
                    return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                if (signInResult.Succeeded == false)
                {
                    loginResponseModel.Status = false;
                    loginResponseModel.Message = "User name or password is incorrect.";

                    return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
                }

                User currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                AccessTokenGenerator accessTokenGenerator = new AccessTokenGenerator(_context, _config, currentUser);

                ApplicationUserTokens userTokens = await accessTokenGenerator.GetToken();
                currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);


                var authRoles = from role in _context.Roles
                                join userRole in _context.UserRoles
                                on role.Id equals userRole.RoleId
                                where userRole.UserId == user.Id
                                select new { RoleName = role.Name };

                foreach (var item in authRoles)
                {
                    loginResponseModel.Roles.Add(item.RoleName);
                };

                loginResponseModel.Status = true;
                loginResponseModel.Message = "User is Logged in.";
                loginResponseModel.Token = new Token()
                {
                    TokenBody = userTokens.Value,
                    ExpireDate = userTokens.ExpireDate,
                    RefreshToken = currentUser.RefreshToken,
                    RefreshTokenExpireDate = currentUser.RefreshTokenExpireDate
                };
                loginResponseModel.UserId = user.Id;
                loginResponseModel.Email = loginModel.Email;
                loginResponseModel.UserName = user.UserName;
                loginResponseModel.FirstName = user.FirstName;
                loginResponseModel.LastName = user.LastName;


                return new SuccessDataResult<LoginResponseModel>(loginResponseModel);
            }
            catch (Exception ex)
            {
                loginResponseModel.Status = false;
                loginResponseModel.Message = ex.Message;

                return new ErrorDataResult<LoginResponseModel>(loginResponseModel);
            }
        }

        public Task<IDataResult<LoginResponseModel>> Register(RegisterModel registerModel)
        {
            throw new NotImplementedException();
        }

        IDataResult<AuthMeResponseModel> IAuthorizationService.AuthMe(string token)
        {
            AuthMeResponseModel authMeResponseModel = new AuthMeResponseModel();

            var roles = from usro in _context.UserRoles
                        join to in _context.UserTokens
                        on usro.UserId equals to.UserId
                        join us in _context.Users
                        on usro.UserId equals us.Id
                        join ro in _context.Roles
                        on usro.RoleId equals ro.Id
                        where usro.UserId == us.Id
                        select new { Roles = ro.Name };

            List<string> roleList = new List<string>();

            foreach (var item in roles)
            {
                roleList.Add(item.Roles);
            }

            roleList = roleList.Distinct().ToList();

            var model = 
                       from u in _context.Users
                       join t in _context.UserTokens
                       on u.Id equals t.UserId
                       where t.Value == token
                       select new AuthMeResponseModel()
                       {
                           UserId = u.Id,
                           Email = u.Email,
                           UserName = u.UserName,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Roles = roleList
                       };

            authMeResponseModel = model.First();

            return new SuccessDataResult<AuthMeResponseModel>(authMeResponseModel);
        }
    }
}
