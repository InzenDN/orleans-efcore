using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TheoryEngineers.Identity.Grains.UserStore;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IUserClaimStore<User>
    {
        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserClaimStoreGrain>(user.Id);
            return grain.AddClaimsAsync(user, claims);
        }

        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserClaimStoreGrain>(user.Id);
            return grain.GetClaimsAsync(user);
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserClaimStoreGrain>(0);
            return grain.GetUsersForClaimAsync(claim);
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserClaimStoreGrain>(user.Id);
            return grain.RemoveClaimsAsync(user, claims);
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var grain = _Client.GetGrain<IUserClaimStoreGrain>(user.Id);
            return grain.ReplaceClaimAsync(user, claim, newClaim);
        }
    }
}
