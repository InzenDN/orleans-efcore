using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    public interface IUserEmailStoreGrain : IGrainWithIntegerKey
    {
        Task<User> FindByEmailAsync(string normalizedEmail);
    }
}
