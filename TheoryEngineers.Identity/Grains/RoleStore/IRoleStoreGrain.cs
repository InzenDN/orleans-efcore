using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Orleans;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.RoleStore
{
    public interface IRoleStoreGrain : IGrainWithIntegerKey 
    {
        Task<IdentityResult> CreateAsync(Role role);
        Task<IdentityResult> DeleteAsync(Role role);
        Task<Role> FindByIdAsync(string roleId);
        Task<Role> FindByNameAsync(string normalizedRoleName);
        Task<IdentityResult> UpdateAsync(Role role);
    }
}