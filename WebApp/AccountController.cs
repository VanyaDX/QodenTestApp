using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace WebApp
{
    // TODO 5: unauthorized users should receive 401 status code and should be redirected to Login endpoint   
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public ValueTask<Account> Get()
        {
            return _accountService.LoadOrCreateAsync(GetIdFromCookie() /* TODO 3: Get user id from cookie -- Solved.*/);
        }

        //TODO 6: Get user id from cookie -- Solved.
        private string GetIdFromCookie()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                HttpContext.Response.StatusCode = 401;
            return id;
        }

        //TODO 7: Endpoint should works only for users with "Admin" Role -- not sure that enough
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task. -- Solved.
            var account = await Get();
            account.Counter++;
            _accountService.UpdateOrCreate(account);
        }
    }
}