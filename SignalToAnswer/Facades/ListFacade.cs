using SignalToAnswer.Constants;
using SignalToAnswer.Form;
using SignalToAnswer.Mappers.Option;
using SignalToAnswer.Options;
using SignalToAnswer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Facades
{
    public class ListFacade
    {
        private readonly UserOptionMapper _userOptionMapper;
        private readonly GroupService _groupService;
        private readonly UserService _userService;

        public ListFacade(UserOptionMapper userOptionMapper, GroupService groupService, UserService userService)
        {
            _userOptionMapper = userOptionMapper;
            _groupService = groupService;
            _userService = userService;
        }

        public async Task<List<UserOption>> SearchInviteList(InviteSearchForm form, string username)
        {
            var group = await _groupService.GetOneUnique(GroupType.MAIN_LOBBY);
            var users = (await _userService.GetAll(form.Username, RoleType.USER, group.Id.Value))
                .Where(u => !u.UserName.Equals(username))
                .ToList();

            return _userOptionMapper.Map(users);
        }
    }
}
