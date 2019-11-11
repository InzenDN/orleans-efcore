using Microsoft.AspNetCore.Identity;
using System.Linq;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IQueryableUserStore<User>
    {
        public IQueryable<User> Users { get; }
    }
}
