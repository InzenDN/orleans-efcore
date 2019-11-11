using Microsoft.AspNetCore.Identity;
using Orleans;
using System.Threading;
using System.Threading.Tasks;
using TheoryEngineers.Identity.Grains.RoleStore;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.RoleStore
{
    public class OrleansRoleStore : IRoleStore<Role>
    {
        private readonly IClusterClient _Client;

        public OrleansRoleStore(IClusterClient client)
        {
            _Client = client;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IRoleStoreGrain>(role.Id);
            var result = await grain.CreateAsync(role);

            if (result.Succeeded)
                role.Id = (await FindByNameAsync(role.NormalizedName, CancellationToken.None)).Id;

            return result;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IRoleStoreGrain>(role.Id);            
            return await grain.DeleteAsync(role);
        }

        public void Dispose()
        {
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IRoleStoreGrain>(int.Parse(roleId));
            return await grain.FindByIdAsync(roleId);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IRoleStoreGrain>(0);
            return await grain.FindByNameAsync(normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName = normalizedName);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name = roleName);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IRoleStoreGrain>(role.Id);
            return await grain.UpdateAsync(role);
        }
    }
}
