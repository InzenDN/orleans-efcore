using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheoryEngineers.Identity.Grains.UserStore;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IUserLockoutStore<User>
    {
        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.GetAccessFailedCountAsync(user);
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.GetLockoutEnabledAsync(user);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.GetLockoutEndDateAsync(user);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount++);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.IncrementAccessFailedCountAsync(user);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount = 0);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.ResetAccessFailedCountAsync(user);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled = enabled);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.SetLockoutEnabledAsync(user, enabled);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd = lockoutEnd);
            //var grain = _Client.GetGrain<IUserLockoutStoreGrain>(user.Id);
            //return grain.SetLockoutEndDateAsync(user, lockoutEnd);
        }
    }
}
