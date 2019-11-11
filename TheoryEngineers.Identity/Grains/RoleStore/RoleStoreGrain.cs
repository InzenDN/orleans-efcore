using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;
using System.Linq;
using TheoryEngineers.Models.Data;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Identity.Grains.RoleStore
{
    [StatelessWorker]
    public class RoleStoreGrain : Grain, IRoleStoreGrain
    {
        private readonly DataContext _Context;

        public RoleStoreGrain(DataContext context)
        {
            _Context = context;
        }

        public async Task<IdentityResult> CreateAsync(Role role)
        {
            try
            {
                await _Context.Roles.AddAsync(role);
                await _Context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IdentityResult.Failed(new IdentityError() { Code = ex.StackTrace, Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(Role role)
        {
            try
            {
                _Context.Roles.Remove(role);
                await _Context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IdentityResult.Failed(new IdentityError() { Code = ex.StackTrace, Description = ex.Message });
            }
            
        }

        public async Task<Role> FindByIdAsync(string roleId)
        {
            var query = from role in _Context.Roles
                        where role.Id == int.Parse(roleId)
                        select role;

            return await query.FirstOrDefaultAsync();            
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName)
        {
            var query = from role in _Context.Roles
                        where role.NormalizedName == normalizedRoleName
                        select role;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IdentityResult> UpdateAsync(Role role)
        {
            try
            {
                _Context.Roles.Update(role);
                await _Context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IdentityResult.Failed(new IdentityError() { Code = ex.StackTrace, Description = ex.Message });
            }            
        }
    }
}
