using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTO
{
    public class ApiRequestInfo
    {
        public bool IsAuthenticated { get; set; } = false;
        public LoggedUser? LoggedUser { get; set; }

    }
}
