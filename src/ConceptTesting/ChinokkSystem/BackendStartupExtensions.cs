using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace ChinookSystem
{
    public static class BackendStartupExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services, 
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<ChinookContext>(options);
            services.AddTransient<MediaTypeServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new MediaTypeServices(context);
            });
            services.AddTransient<GenreServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new GenreServices(context);
            });
            services.AddTransient<AlbumServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new AlbumServices(context);
            });
            services.AddTransient<AboutServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new AboutServices(context);
            });

        }
    }
}
