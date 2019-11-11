using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheoryEngineers.Identity.Grains.UserStore;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IUserLoginStore<User>
    {
        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserLoginStoreGrain>(user.Id);
            return grain.AddLoginAsync(user, login);
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserLoginStoreGrain>(0);
            return grain.FindByLoginAsync(loginProvider, providerKey);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserLoginStoreGrain>(user.Id);
            return grain.GetLoginsAsync(user);
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserLoginStoreGrain>(user.Id);
            return grain.RemoveLoginAsync(user, loginProvider, providerKey);
        }
    }
}
