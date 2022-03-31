using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSecurity.Models
{
    public interface IIdentifyEmployee
    {
        int? EmployeeId { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}
