using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<IActionResult> LoginUser(LoginModel loginModel);
        public Task<IActionResult> RegisterUser(RegisterModel registerModel);
    }
}
