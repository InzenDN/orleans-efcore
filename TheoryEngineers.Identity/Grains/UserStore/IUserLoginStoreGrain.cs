using Microsoft.AspNetCore.Identity;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    public interface IUserLoginStoreGrain : IGrainWithIntegerKey
    {
        Task AddLoginAsync(User user, UserLoginInfo login);
        Task<User> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IList<UserLoginInfo>> GetLoginsAsync(User user);
        Task RemoveLoginAsync(User user, string loginProvider, string providerKey);
    }
}
