using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheoryEngineers.Identity.Grains.UserStore;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IUserRoleStore<User>
    {
        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserRoleStoreGrain>(user.Id);
            return grain.AddToRoleAsync(user, roleName);
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserRoleStoreGrain>(user.Id);
            return grain.GetRolesAsync(user);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserRoleStoreGrain>(0);
            return grain.GetUsersInRoleAsync(roleName);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserRoleStoreGrain>(user.Id);
            return grain.IsInRoleAsync(user, roleName);
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserRoleStoreGrain>(user.Id);
            return grain.RemoveFromRoleAsync(user, roleName);
        }
    }
}
