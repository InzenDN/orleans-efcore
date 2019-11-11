using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    [StatelessWorker]
    public class UserLockoutStoreGrain : Grain, IUserLockoutStoreGrain
    {
        private readonly DataContext _Context;

        public UserLockoutStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser.AccessFailedCount;

            return query.FirstOrDefaultAsync();
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser.LockoutEnabled;

            return query.FirstOrDefaultAsync();
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser.LockoutEnd;

            return query.FirstOrDefaultAsync();
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser;

            var newUser = await query.FirstOrDefaultAsync();
            newUser.AccessFailedCount++;

            _Context.Users.Update(newUser);
            await _Context.SaveChangesAsync();

            return newUser.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser;

            var newUser = await query.FirstOrDefaultAsync();
            newUser.AccessFailedCount = 0;

            _Context.Users.Update(newUser);
            await _Context.SaveChangesAsync();
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser;

            var newUser = await query.FirstOrDefaultAsync();
            newUser.LockoutEnabled = enabled;

            _Context.Users.Update(newUser);
            await _Context.SaveChangesAsync();
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd)
        {
            var query = from auser in _Context.Users
                        where auser.NormalizedUserName == user.NormalizedUserName || auser.Id == user.Id
                        select auser;

            var newUser = await query.FirstOrDefaultAsync();
            newUser.LockoutEnd = lockoutEnd;

            _Context.Users.Update(newUser);
            await _Context.SaveChangesAsync();
        }
    }
}
