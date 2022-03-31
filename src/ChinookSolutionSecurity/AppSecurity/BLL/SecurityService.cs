
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using AppSecurity.DAL;
using AppSecurity.Models;
#endregion

namespace AppSecurity.BLL
{
    public class SecurityService
    {
        private readonly AppSecurityDbContext _context;

        internal SecurityService(AppSecurityDbContext context)
        {
            _context = context;
        }

        public List<IIdentifyEmployee> ListEmployees()
        {
            var people = from emp in _context.Employees
                         select new StaffMember
                         {
                             EmployeeId = emp.EmployeeId,
                             Email = $"{emp.FirstName}.{emp.LastName}@Chinook.edu.ca",
                             // NOTE: UserName as an email to match the default login page
                             UserName = $"{emp.FirstName}.{emp.LastName}@Chinook.edu.ca"
                             //UserName = $"{emp.FirstName}.{emp.LastName}" // Alternative
                         };
            return people.ToList<IIdentifyEmployee>();
        }

        public string GetEmployeeName(int employeeId)
        {
            string result = "";
            var found = _context.Employees.Find(employeeId);
            if (found != null)
                result = $"{found.FirstName} {found.LastName}";
            return result;
        }
    }
}
