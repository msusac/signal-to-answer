using Microsoft.AspNetCore.SignalR;
using System;

namespace SignalToAnswer.Extensions
{
    public static class HubContextCallerExtensions
    {
        public static int GetGameId(this HubCallerContext context)
        {
            return Int32.Parse(context.GetHttpContext().Request.Query["gameId"]);
        }

        public static string GetUserIdentifier(this HubCallerContext context)
        {
            return context.UserIdentifier;
        }

        public static string GetUsername(this HubCallerContext context)
        {
            return context.User.Identity.Name;
        }
    }
}
