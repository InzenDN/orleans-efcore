using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Stores.UserStore
{
    public partial class OrleansUserStore : IUserTwoFactorStore<User>
    {
        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled = enabled);
        }
    }
}
