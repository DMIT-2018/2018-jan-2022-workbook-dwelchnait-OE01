using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AppSecurity.DAL;
using AppSecurity.BLL;
#endregion


namespace AppSecurity
{
    public static class SecurityExtensions
    {
        public static void AppSecurityBackendDependencies(this IServiceCollection services,
           Action<DbContextOptionsBuilder> options)
        {
            //register the DbContext class in Chinook with the service collection
            services.AddDbContext<AppSecurityDbContext>(options);

            //add any services that you create in the class library
            //  using .AddTransient<T>(....)
            services.AddTransient<SecurityService>((serviceProvider) =>
            {
                var context = serviceProvider.GetRequiredService<AppSecurityDbContext>();
                return new SecurityService(context);
            });
           
        }
    }
}
