using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class UserService
    {
        private readonly GroupService _groupService;
        private readonly UserRepository _userRepository;

        public UserService(GroupService groupService, UserRepository userRepository)
        {
            _groupService = groupService;
            _userRepository = userRepository;
        }

        [Transactional]
        public async Task<User> CreateGuest()
        {
            var username = await GenerateGuestUsername();
            var group = await _groupService.GetOne(GroupType.OFFLINE);

            var guest = new User
            {
                Id = Guid.Empty,
                UserName = username,
                Password = username,
                Role = RoleType.GUEST
            };

            guest.Connection = new Connection { User = guest, Group = group };

            await _userRepository.Save(guest);

            return await _userRepository.FindOneById(guest.Id);
        }

        [Transactional]
        public async Task<User> CreateUser(string username, string email, string password)
        {
            var group = await _groupService.GetOne(GroupType.OFFLINE);

            var user = new User
            {
                Id = Guid.Empty,
                UserName = username,
                Email = email,
                Password = password,
                Role = RoleType.USER
            };
            user.Connection = new Connection { User = user, Group = group };

            await _userRepository.Save(user);

            return await _userRepository.FindOneById(user.Id);
        }

        public async Task<List<User>> GetAll(string roleName, int groupId)
        {
            return await _userRepository.FindAllByRole_NameAndGroup_Id(roleName, groupId);
        }

        public async Task<List<User>> GetAll(string username, string roleName, int groupId)
        {
            return await _userRepository.FindAllByUsernameStartsWithAndRole_NameAndGroup_Id(username, roleName, groupId);
        }

        public async Task<User> GetOne(Guid id)
        {
            var user = await _userRepository.FindOneById(id);

            if (user == null)
            {
                throw new EntityNotFoundException("Selected user does not exist!");
            }

            return user;
        }

        public async Task<User> GetOne(Guid id, string role)
        {
            var user = await _userRepository.FindOneByIdAndRole_Name(id, role);

            if (user == null)
            {
                throw new EntityNotFoundException("Selected user does not exist!");
            }

            return user;
        }

        public async Task<User> GetOne(string username)
        {
            var user = await _userRepository.FindOneByUsername(username);

            if (user == null)
            {
                throw new EntityNotFoundException("Selected user does not exist!");
            }

            return user;
        }

        public async Task<User> GetOne(string username, string role)
        {
            var user = await _userRepository.FindOneByUsernameAndRole_Name(username, role);

            if (user == null)
            {
                throw new EntityNotFoundException("Selected user does not exist!");
            }

            return user;
        }

        [Transactional]
        public async Task Deactivate(User user)
        {
            user.Active = false;
            await _userRepository.Save(user);
        }

        private async Task<string> GenerateGuestUsername()
        {
            var items = await _userRepository.FindAllByRole_Name(RoleType.GUEST);

            var count = items.Count() + 1;

            return string.Format("GuestUser-{0}", count);
        }
    }
}
