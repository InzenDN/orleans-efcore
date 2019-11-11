using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    public interface IUserRoleStoreGrain : IGrainWithIntegerKey
    {
        Task AddToRoleAsync(User user, string roleName);
        Task<IList<string>> GetRolesAsync(User user);
        Task<IList<User>> GetUsersInRoleAsync(string roleName);
        Task RemoveFromRoleAsync(User user, string roleName);
        Task<bool> IsInRoleAsync(User user, string roleName);
    }
}