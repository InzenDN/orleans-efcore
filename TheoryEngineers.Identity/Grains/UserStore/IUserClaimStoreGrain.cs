using Orleans;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    public interface IUserClaimStoreGrain : IGrainWithIntegerKey
    {
        Task AddClaimsAsync(User user, IEnumerable<Claim> claims);
        Task<IList<Claim>> GetClaimsAsync(User user);
        Task<IList<User>> GetUsersForClaimAsync(Claim claim);
        Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims);
        Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim);
    }
}
