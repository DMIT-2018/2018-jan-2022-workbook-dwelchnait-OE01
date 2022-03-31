#region Additional Namespaces
using AppSecurity.Models;
using Microsoft.AspNetCore.Identity;
#endregion


namespace WebApp.Data
{
    public class ApplicationUser : IdentityUser, IIdentifyEmployee
    {
        public int? EmployeeId { get; set; }
    }
}
