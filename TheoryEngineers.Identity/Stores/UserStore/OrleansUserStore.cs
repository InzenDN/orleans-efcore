using Microsoft.AspNetCore.Identity;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using TheoryEngineers.Identity.Grains.UserStore;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IUserStore<User>
    {
        private readonly IClusterClient _Client;

        public OrleansUserStore(IClusterClient client, DataContext context)
        {
            _Client = client;
            Users = from user in context.Users
                    select user;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserStoreGrain>(user.Id);
            var result = await grain.CreateAsync(user);

            if (result.Succeeded)
                user.Id = (await FindByNameAsync(user.NormalizedUserName, CancellationToken.None)).Id;

            return result;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserStoreGrain>(user.Id);
            return await grain.DeleteAsync(user);
        }

        public void Dispose()
        {
            
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserStoreGrain>(int.Parse(userId));
            return await grain.FindByIdAsync(userId);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserStoreGrain>(0);
            return await grain.FindByNameAsync(normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName = normalizedName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName = userName);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserStoreGrain>(user.Id);
            return await grain.UpdateAsync(user);
        }
    }
}
