using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.UserStore
{
    [StatelessWorker]
    public class UserRoleStoreGrain : Grain, IUserRoleStoreGrain
    {
        private readonly DataContext _Context;

        public UserRoleStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            var roleId = await (from role in _Context.Roles
                         where role.NormalizedName == roleName.ToUpper()
                         select role.Id).FirstOrDefaultAsync();

            var userRole = new UserRole
            {
                RoleId = roleId,
                UserId = user.Id
            };

            try
            {
                await _Context.UserRoles.AddAsync(userRole);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddToRoleAsync: {ex}");
            }            
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            var query = from userRole in _Context.UserRoles
                        where userRole.UserId == user.Id
                        join role in _Context.Roles on userRole.RoleId equals role.Id
                        select role.Name;

            return Task.FromResult(query.ToList() as IList<string>);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName)
        {
            var query = from role in _Context.Roles
                        where role.NormalizedName == roleName.ToUpper()
                        join userRole in _Context.UserRoles on role.Id equals userRole.RoleId
                        join user in _Context.Users on userRole.UserId equals user.Id
                        select user;

            return Task.FromResult(query.ToList() as IList<User>);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var query = from role in _Context.Roles
                        where role.NormalizedName == roleName.ToUpper()
                        join userRole in _Context.UserRoles on role.Id equals userRole.RoleId
                        where userRole.UserId == user.Id
                        select userRole;

            return Task.FromResult(query.ToList().Count > 0);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            int roleId = (from role in _Context.Roles
                          where role.NormalizedName == roleName.ToUpper()
                          select role.Id).FirstOrDefault();

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            };

            try
            {
                _Context.UserRoles.Remove(userRole);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
