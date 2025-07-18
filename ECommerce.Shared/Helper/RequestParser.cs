using ECommerce.DTO;
using ECommerce.Shared.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Helper
{
    public static class RequestParser
    {
        public static ApiRequestInfo ParseInfo(HttpContext context)
        {
            return new ApiRequestInfo
            {
                IsAuthenticated = IsAuthenticated(context),
                LoggedUser = LoggedUser(context)
            };
        }
        public static bool IsAuthenticated(HttpContext context) => context.User.Identity?.IsAuthenticated ?? false;

        public static LoggedUser? LoggedUser(HttpContext context) {
            if (!IsAuthenticated(context)) return null;

            _ = long.TryParse(context.User.FindFirst(x => x.Type == ClaimConstants.SystemUserId)?.Value, out long systemUserId);

            //Sometime claim "sub" is not mapping to "nameidentifier"
            var aspNetUserId = context.User.FindFirst(x => x.Type == ClaimConstants.Subject)?.Value ?? "";
            if (string.IsNullOrEmpty(aspNetUserId))
            {
                aspNetUserId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            return new LoggedUser
            {
                AspNetUserId = aspNetUserId ?? string.Empty,
                SystemUserId = systemUserId,
             };
        }

    }
}
