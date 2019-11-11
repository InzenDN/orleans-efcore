using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    [StatelessWorker]
    public class UserLoginStoreGrain : Grain, IUserLoginStoreGrain
    {
        private readonly DataContext _Context;

        public UserLoginStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            var userLogin = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName
            };

            _Context.UserLogins.AddAsync(userLogin);
            return _Context.SaveChangesAsync();
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            var query = from userLogin in _Context.UserLogins
                        where userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey
                        join user in _Context.Users on userLogin.UserId equals user.Id
                        select user;

            return Task.FromResult(query.FirstOrDefault());
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            var query = from userLogin in _Context.UserLogins
                        where userLogin.UserId == user.Id
                        select userLogin;

            return Task.FromResult((IList<UserLoginInfo>)query.ToList());
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey)
        {
            var query = from userLogin in _Context.UserLogins
                        where userLogin.UserId == user.Id && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey
                        select userLogin;

            var userLoginToDelete = await query.FirstOrDefaultAsync();

            _Context.UserLogins.Remove(userLoginToDelete);
        }
    }
}
