using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class ConnectionRepository
    {
        private readonly DataContext _dataContext;

        public ConnectionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Connection>> FindAllByGroupId(int groupId)
        {
            return await _dataContext.Connections.Where(c => c.GroupId.Equals(groupId) && c.Active.Equals(true)).ToListAsync();
        }

        public async Task<Connection> FindOneByUserId(Guid userId)
        {
            return await _dataContext.Connections.SingleOrDefaultAsync(c => c.UserId.Equals(userId) && c.Active.Equals(true));
        }

        public async Task<Connection> FindOneByGroupIdAndUserId(int groupId, Guid userId)
        {
            return await _dataContext.Connections.SingleOrDefaultAsync(c => c.GroupId.Equals(groupId) && c.UserId.Equals(userId) && c.Active.Equals(true));
        }

        public async Task<Connection> Save(Connection connection)
        {
            if (connection.Id == null)
            {
                await _dataContext.Connections.AddAsync(connection);
            }
            else
            {
                connection.UpdatedAt = DateTime.Now;

                _dataContext.Connections.Update(connection);
            }

            await _dataContext.SaveChangesAsync();

            return connection;
        }
    }
}
