using Microsoft.AspNetCore.Identity;
using Orleans;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    public interface IUserStoreGrain : IGrainWithIntegerKey
    {
        Task<IdentityResult> CreateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByNameAsync(string normalizedUserName);
        Task<IdentityResult> UpdateAsync(User user);
    }
}
