using System.Threading.Tasks;

namespace WebApp
{
    class AccountService : IAccountService
    {
        // TODO 4: keep the cache up to date in accordance with DB (data about accounts in db and cache should be fully synced) -- didn't understand, should I use IMemoryCache?
        private readonly IAccountCache _cache;
        private readonly IAccountDatabase _db;

        public AccountService(IAccountCache cache, IAccountDatabase db)
        {
            _cache = cache;
            _db = db;
        }
        
        public Account GetFromCache(long id)
        {
            if (_cache.TryGetValue(id, out var account))
            {
                return account;
            }

            return null;
        }
        
        public async ValueTask<Account> LoadOrCreateAsync(string id)
        {
            if (!_cache.TryGetValue(id, out var account))
            {
                account = await _db.GetOrCreateAccountAsync(id);
                _cache.AddOrUpdate(account);
            }

            return account;
        }
        
        public async ValueTask<Account> LoadOrCreateAsync(long id)
        {
            if (!_cache.TryGetValue(id, out var account))
            {
                account = await _db.GetOrCreateAccountAsync(id);
                _cache.AddOrUpdate(account);
            }

            return account;
        }

        public void UpdateOrCreate(Account account)
        {
            _cache.AddOrUpdate(account);
        }
    }
}