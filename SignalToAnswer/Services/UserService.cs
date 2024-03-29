﻿using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class UserService
    {
        private readonly GroupService _groupService;
        private readonly ConnectionRepository _connectionRepository;
        private readonly UserRepository _userRepository;

        public UserService(GroupService groupService, ConnectionRepository connectionRepository, UserRepository userRepository)
        {
            _groupService = groupService;
            _connectionRepository = connectionRepository;
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

            guest = await _userRepository.Save(guest);

            var connection = new Connection { GroupId = group.Id.Value, UserId = guest.Id };
            await _connectionRepository.Save(connection);

            return await _userRepository.FindOneById(guest.Id);
        }

        [Transactional]
        public async Task<User> CreateHostBot()
        {
            var username = await GenerateHostBotUsername();

            var bot = new User
            {
                Id = Guid.Empty,
                UserName = username,
                Password = username,
                Role = RoleType.HOST_BOT
            };

            await _userRepository.Save(bot);

            return await _userRepository.FindOneById(bot.Id);
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

            user = await _userRepository.Save(user);

            var connection = new Connection { GroupId = group.Id.Value, UserId = user.Id };
            await _connectionRepository.Save(connection);

            return await _userRepository.FindOneById(user.Id);
        }

        public async Task<List<User>> GetAll(string roleName)
        {
            return await _userRepository.FindAllByRole_Name(roleName);
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
            user.UserRole.Active = false;
            await _userRepository.SaveAlt(user);
        }

        [Transactional]
        public async Task DeactivateAlt(User user)
        {
            user.Active = false;
            await _userRepository.SaveAlt(user);
        }

        private async Task<string> GenerateGuestUsername()
        {
            var items = await _userRepository.FindAllByRole_NameAndActiveExcluded(RoleType.GUEST);

            var count = items.Count + 1;

            return string.Format("GuestUser-{0}", count);
        }

        private async Task<string> GenerateHostBotUsername()
        {
            var items = await _userRepository.FindAllByRole_NameAndActiveExcluded(RoleType.HOST_BOT);

            var count = items.Count + 1;

            return string.Format("HostBotUser-{0}", count);
        }
    }
}
