using Business.Security.Models;
using DataAccess.Concrete.EntityFramework;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security
{
    public class AccessTokenGenerator
    {
        public ContextDb _context { get; set; }
        public IConfiguration _config { get; set; }
        public User _user { get; set; }

        public AccessTokenGenerator(ContextDb context, IConfiguration config, User user)
        {
            _context = context;
            _config = config;
            _user = user;
        }

        private Token GeneterateToken()
        {
            DateTime expireDate = DateTime.Now.AddMinutes(15);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Application:Secret"]);
            var authRoles = from role in _context.Roles
                            join userRole in _context.UserRoles
                            on role.Id equals userRole.RoleId
                            where userRole.UserId == _user.Id
                            select new { RoleName = role.Name };
            var authClaims = new List<Claim>();
            foreach (var item in authRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item.RoleName));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config["Application:Audience"],
                Issuer = _config["Application:Issuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, _user.Id),
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.Email, _user.Email),
                }),

                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            tokenDescriptor.Subject.AddClaims(authClaims);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Token tokenInfo = new Token();
            tokenInfo.TokenBody = tokenString;
            tokenInfo.ExpireDate = expireDate;
            tokenInfo.RefreshToken = Guid.NewGuid().ToString();

            _user.RefreshToken = tokenInfo.RefreshToken;
            _user.RefreshTokenExpireDate = tokenInfo.ExpireDate.AddMinutes(5);
            _context.SaveChanges();

            return tokenInfo;
        }

        public ApplicationUserTokens GetToken()
        {
            ApplicationUserTokens userTokens = null;
            Token token = null;

            if (_context.ApplicationUserTokens.Count(x => x.UserId == _user.Id) > 0)
            {
                userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _user.Id);

                if (userTokens.ExpireDate <= DateTime.Now)
                {
                    token = GeneterateToken();

                    userTokens.ExpireDate = token.ExpireDate;
                    userTokens.Value = token.TokenBody;

                    _context.ApplicationUserTokens.Update(userTokens);
                }
            }
            else
            {
                token = GeneterateToken();

                userTokens = new ApplicationUserTokens();

                userTokens.UserId = _user.Id;
                userTokens.LoginProvider = "SystemAPI";
                userTokens.Name = _user.UserName;
                userTokens.ExpireDate = token.ExpireDate;
                userTokens.Value = token.TokenBody;

                _context.ApplicationUserTokens.Add(userTokens);
            }
            _context.SaveChangesAsync();

            return userTokens;
        }

        public async Task<bool> DeleteToken()
        {
            bool result = true;

            try
            {
                if (_context.ApplicationUserTokens.Count(x => x.UserId == _user.Id) > 0)
                {
                    ApplicationUserTokens userTokens = userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _user.Id);

                    _context.ApplicationUserTokens.Remove(userTokens);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}