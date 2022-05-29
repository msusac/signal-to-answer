using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class GroupRepository
    {
        private readonly DataContext _dataContext;

        public GroupRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Group> FindOneByGroupName(string group)
        {
            return await _dataContext.Groups.SingleOrDefaultAsync(g => g.GroupName.Equals(group) && g.Active.Equals(true));
        }

        public async Task<Group> FindOneById(int id) 
        {
            return await _dataContext.Groups.SingleOrDefaultAsync(g => g.Id.Equals(id) && g.Active.Equals(true));
        }

        public async Task<Group> FindOneByGroupTypeAndIsUniqueTrue(int groupType)
        {
            return await _dataContext.Groups.SingleOrDefaultAsync(g => g.GroupType.Equals(groupType) && g.IsUnique.Equals(true) && g.Active.Equals(true));
        }

        public async Task<Group> Save(Group group)
        {
            if (group.Id == null)
            {
                await _dataContext.Groups.AddAsync(group);
            }
            else
            {
                group.UpdatedAt = DateTime.Now;

                _dataContext.Groups.Update(group);
            }

            await _dataContext.SaveChangesAsync();

            return group;
        }
    }
}
