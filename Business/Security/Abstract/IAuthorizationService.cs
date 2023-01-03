using Business.Security.Models;
using Core.Utilities.Results;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Abstract
{
    public interface IAuthorizationService
    {
        IDataResult<AuthMeResponseModel> AuthMe(string token);
        Task<IDataResult<LoginResponseModel>> Login(LoginModel loginModel);
        Task<IDataResult<RegisterResponseModel>> RegisterUser(RegisterModel registerModel);
        Task<IDataResult<RegisterResponseModel>> RegisterAdmin(RegisterModel registerModel);
        Task<IDataResult<RefreshTokenResponseModel>> RefreshToken(string token);
    }
}
