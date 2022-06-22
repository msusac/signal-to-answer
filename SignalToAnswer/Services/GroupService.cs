using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Repositories;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class GroupService
    {
        private readonly GroupRepository _groupRepository;

        public GroupService(GroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Group> CreateInGameSoloGroup(Game game)
        {
            var group = new Group
            {
                GroupName = string.Format("IN_GAME_SOLO_{0}", game.Id),
                GroupType = GroupType.IN_GAME_SOLO,
                IsUnique = false
            };

            return await _groupRepository.Save(group);
        }

        public async Task<Group> CreateInGamePublicGroup(Game game)
        {
            var group = new Group
            {
                GroupName = string.Format("IN_GAME_PUBLIC_{0}", game.Id),
                GroupType = GroupType.IN_GAME_PUBLIC,
                IsUnique = false
            };

            return await _groupRepository.Save(group);
        }

        public async Task<Group> CreateInGamePrivateGroup(Game game)
        {
            var group = new Group
            {
                GroupName = string.Format("IN_GAME_PRIVATE_{0}", game.Id),
                GroupType = GroupType.IN_GAME_PRIVATE,
                IsUnique = false
            };

            return await _groupRepository.Save(group);
        }

        public async Task<Group> CreateInviteLobbyGroup(Game game)
        {
            var group = new Group
            {
                GroupName = string.Format("INVITE_LOBBY_{0}", game.Id),
                GroupType = GroupType.INVITE_LOBBY,
                IsUnique = false
            };

            return await _groupRepository.Save(group);
        }

        public async Task<Group> GetOne(int id)
        {
            var group = await _groupRepository.FindOneById(id);

            if (group == null)
            {
                throw new EntityNotFoundException("Selected group does not exist!");
            }

            return group;
        }

        public async Task<Group> GetOneInGame(int id, int gameType)
        {
            var inGroupType = GetInGameType(gameType);

            var group = await _groupRepository.FindOneByGroupName(inGroupType + id);

            if (group == null)
            {
                throw new EntityNotFoundException("Selected group does not exist!");
            }

            return group;
        }

        private string GetInGameType(int gameType)
        {
            var inGameType= "IN_GAME_SOLO_";

            if (gameType == GameType.PUBLIC)
            {
                inGameType = "IN_GAME_PUBLIC_";
            }
            else if (gameType == GameType.PRIVATE)
            {
                inGameType = "IN_GAME_PRIVATE_";
            }

            return inGameType;
        }

        public async Task<Group> GetOneInviteLobby(int id)
        {
            var group = await _groupRepository.FindOneByGroupName("INVITE_LOBBY_" + id);

            if (group == null)
            {
                throw new EntityNotFoundException("Selected group does not exist!");
            }

            return group;
        }

        public async Task<Group> GetOneUnique(int groupType)
        {
            var group = await _groupRepository.FindOneByGroupTypeAndIsUniqueTrue(groupType);

            if (group == null)
            {
                throw new EntityNotFoundException("Selected group does not exist!");
            }

            return group;
        }
    }
}
