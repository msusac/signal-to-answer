using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class RoleRepository
    {
        private readonly DataContext _dataContext;

        public RoleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Role> FindOneByUserId(Guid userId)
        {
            var userRole = await _dataContext.UserRoles.SingleOrDefaultAsync(ur => ur.UserId.Equals(userId) && ur.Active.Equals(true));

            if (userRole == null)
            {
                return null;
            }

            return await _dataContext.Roles.SingleOrDefaultAsync(r => r.Id.Equals(userRole.Role));
        }
    }
}
