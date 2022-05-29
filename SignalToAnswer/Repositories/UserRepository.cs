using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class UserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> FindAllByRole_Name(string roleName)
        {
            return await _userManager.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRole.Role.Name.Equals(roleName) && u.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<List<User>> FindAllByRole_NameAndGroup_Id(string roleName, int groupId)
        {
            return await _userManager.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Connection)
                .ThenInclude(c => c.Group)
                .Where(u => u.UserRole.Role.Name.Equals(roleName) && u.Connection.Group.Id.Equals(groupId) && u.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<List<User>> FindAllByUsernameStartsWithAndRole_NameAndGroup_Id(string username, string roleName, int groupId)
        {
            return await _userManager.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Connection)
                .ThenInclude(c => c.Group)
                .Where(u => u.UserName.ToLower().StartsWith(username.ToLower()) && u.UserRole.Role.Name.Equals(roleName) && u.Connection.Group.Id.Equals(groupId) && u.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<User> FindOneByEmailAndRole_Name(string email, string roleName)
        {
            return await _userManager.Users
               .Include(u => u.UserRole)
               .ThenInclude(ur => ur.Role)
               .SingleOrDefaultAsync(u => u.Email.Equals(email) && u.UserRole.Role.Name.Equals(roleName) && u.Active.Equals(true));
        }

        public async Task<User> FindOneById(Guid id)
        {
            return await _userManager.Users
               .Include(u => u.UserRole)
               .ThenInclude(ur => ur.Role)
               .SingleOrDefaultAsync(u => u.Id.Equals(id) && u.Active.Equals(true));
        }

        public async Task<User> FindOneByIdAndRole_Name(Guid id, string roleName)
        {
            return await _userManager.Users
               .Include(u => u.UserRole)
               .ThenInclude(ur => ur.Role)
               .SingleOrDefaultAsync(u => u.Id.Equals(id) && u.UserRole.Role.Name.Equals(roleName) && u.Active.Equals(true));
        }

        public async Task<User> FindOneByUsername(string username)
        {
            return await _userManager.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.UserName.Equals(username) && u.Active.Equals(true));
        }

        public async Task<User> FindOneByUsernameAndGroup_IdAndRole_Name(string username, int groupId, string roleName)
        {
            return await _userManager.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Connection)
                .ThenInclude(ug => ug.Group)
                .SingleOrDefaultAsync(u => u.UserName.Equals(username) && u.Connection.Group.Id.Equals(groupId) &&
                    u.UserRole.Role.Name.Equals(roleName) && u.Active.Equals(true));
        }

        public async Task<User> FindOneByUsernameAndRole_Name(string username, string roleName)
        {
            return await _userManager.Users
               .Include(u => u.UserRole)
               .ThenInclude(ur => ur.Role)
               .SingleOrDefaultAsync(u => u.UserName.Equals(username) && u.UserRole.Role.Name.Equals(roleName) && u.Active.Equals(true));
        }

        public async Task<User> Save(User user)
        {
            if (user.Id.Equals(Guid.Empty))
            {
                await _userManager.CreateAsync(user, user.Password);
                await _userManager.AddToRoleAsync(user, user.Role);
            }
            else
            {
                user.UpdatedAt = DateTime.Now;

                await _userManager.UpdateAsync(user);
            }

            return user;
        }
    }
}
