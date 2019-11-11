using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    [StatelessWorker]
    public class UserClaimStoreGrain : Grain, IUserClaimStoreGrain
    {
        private readonly DataContext _Context;

        public UserClaimStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims)
        {
            var userClaims = new List<UserClaim>();

            foreach(Claim claim in claims)
            {
                userClaims.Add(new UserClaim()
                {
                    UserId = user.Id,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }
            _Context.UserClaims.AddRangeAsync(userClaims);
            return _Context.SaveChangesAsync();
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            var query = from userClaim in _Context.UserClaims
                        where userClaim.UserId == user.Id
                        select userClaim;

            var userClaims = query.ToList();

            var claims = new List<Claim>();

            foreach(UserClaim claim in userClaims)
            {
                claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            return Task.FromResult((IList<Claim>)claims);
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim)
        {
            var query = from userClaim in _Context.UserClaims
                        where userClaim.ClaimType == claim.Type && userClaim.ClaimValue == claim.Value
                        join user in _Context.Users on userClaim.UserId equals user.Id
                        select user;

            return Task.FromResult((IList<User>)query.ToList());
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims)
        {
            var userClaims = new List<UserClaim>();

            foreach (Claim claim in claims)
            {
                var query = from userClaim in _Context.UserClaims
                            where userClaim.UserId == user.Id && userClaim.ClaimType == claim.Type && userClaim.ClaimValue == claim.Value
                            select userClaim;

                userClaims.Add(query.FirstOrDefault());
            }

            _Context.RemoveRange(userClaims);
            return _Context.SaveChangesAsync();
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim)
        {
            var query = from userClaim in _Context.UserClaims
                        where userClaim.UserId == user.Id && userClaim.ClaimType == claim.Type && userClaim.ClaimValue == claim.Value
                        select userClaim;

            var newUserClaim = query.FirstOrDefault();
            newUserClaim.ClaimType = newClaim.Type;
            newUserClaim.ClaimValue = newClaim.Value;

            _Context.UserClaims.Update(newUserClaim);
            return _Context.SaveChangesAsync();
        }
    }
}
