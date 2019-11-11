using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    [StatelessWorker]
    public class UserStoreGrain : Grain, IUserStoreGrain 
    {
        private readonly DataContext _Context;

        public UserStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public async Task<IdentityResult> CreateAsync(User user)
        {
            try
            {
                await _Context.Users.AddAsync(user);
                await _Context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IdentityResult.Failed(new IdentityError() { Code = ex.StackTrace, Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            try
            {
                _Context.Users.Remove(user);
                await _Context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IdentityResult.Failed(new IdentityError() { Code = ex.StackTrace, Description = ex.Message });
            }
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            return await _Context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
        }

        public async Task<User> FindByNameAsync(string normalizedUserName)
        {
            return await _Context.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            try
            {
                _Context.Users.Update(user);
                await _Context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IdentityResult.Failed(new IdentityError() { Code = ex.StackTrace, Description = ex.Message });
            }
        }
    }
}
