#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
#endregion

namespace ChinookSystem.BLL
{
    public class AboutServices
    {
        //this class needs to be accessed by an "outsider user" (WebApp)
        //therefore the class needs to be public

        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        //obtan the context link from IServiceCollection when this
        //  set of services is injected into the "outside user"
        internal AboutServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Services
        //services are methods

        //qeury to obtain the DbVersion data
        public DbVersionInfo GetDbVersion()
        {
            DbVersionInfo info = _context.DbVersions
                                .Select(x => new DbVersionInfo
                                {
                                    Major = x.Major,
                                    Minor = x.Minor,
                                    Build = x.Build,
                                    ReleaseDate = x.ReleaseDate
                                })
                                .SingleOrDefault();
            return info;
        }
        #endregion
    }
}
