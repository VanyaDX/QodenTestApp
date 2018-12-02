using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }
        
        [HttpPost("sign-in")]
        public async Task Login(string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                //TODO 1: Generate auth cookie for user 'userName' with external id -- Solved.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, account.ExternalId),
                    new Claim(ClaimTypes.Role, account.Role)
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
            //TODO 2: return 404 not found if user not found -- Solved.
            HttpContext.Response.StatusCode = 404;
        }
    }
}