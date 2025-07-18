using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTO
{
    public class LoggedUser
    {
        public string AspNetUserId { get; set; } = string.Empty;
        public long SystemUserId { get; set; }
    }
}
