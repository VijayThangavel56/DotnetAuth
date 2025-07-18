using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Constants
{
    public static class ClaimConstants
    {
        ///<summary>Claim to get AspNetUserId from principal (access token).</summary>
        public const string Subject = "sub";

        ///<summary>Claim to get System user id from principal (access token).</summary>
        public const string SystemUserId = "system_user_id";

        ///<summary>Claim to get Name from principal (access token).</summary>
        public const string Name = "name";

        ///<summary>Claim to get Email from principal (access token).</summary>
        public const string Email = "email";
    }
}
