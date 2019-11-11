using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    public interface IUserLockoutStoreGrain : IGrainWithIntegerKey
    {
        Task<int> GetAccessFailedCountAsync(User user);
        Task<bool> GetLockoutEnabledAsync(User user);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(User user);
        Task<int> IncrementAccessFailedCountAsync(User user);
        Task ResetAccessFailedCountAsync(User user);
        Task SetLockoutEnabledAsync(User user, bool enabled);
        Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd);
    }
}
