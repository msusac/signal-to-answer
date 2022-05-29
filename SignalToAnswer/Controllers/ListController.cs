using Microsoft.AspNetCore.Mvc;
using SignalToAnswer.Extensions;
using SignalToAnswer.Facades;
using SignalToAnswer.Form;
using SignalToAnswer.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Controllers
{
    [ApiController]
    [Route("api/list")]
    public class ListController : ControllerBase
    {
        private readonly ListFacade _listFacade;

        public ListController(ListFacade listFacade)
        {
            _listFacade = listFacade;
        }

        [HttpPost("invite-search")]
        public async Task<List<UserOption>> SearchInviteList([FromBody] InviteSearchForm form)
        {
            return await _listFacade.SearchInviteList(form, User.GetUsername());
        }
    }
}
