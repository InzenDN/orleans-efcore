using Orleans;
using Orleans.Concurrency;
using System.Linq;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    [StatelessWorker]
    public class UserEmailStoreGrain : Grain, IUserEmailStoreGrain
    {
        private readonly DataContext _Context;

        public UserEmailStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public Task<User> FindByEmailAsync(string normalizedEmail)
        {
            var query = from user in _Context.Users
                        where user.NormalizedEmail == normalizedEmail
                        select user;

            return Task.FromResult(query.FirstOrDefault());
        }
    }
}
