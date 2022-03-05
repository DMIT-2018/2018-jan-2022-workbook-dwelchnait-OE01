using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endregion


namespace ChinookSystem.BLL
{
    public class AboutServices
    {
        #region Constructor and Dependencies
        private readonly ChinookContext _context;
        internal AboutServices(ChinookContext context)
        {
            this._context = context;
        }
        #endregion

        #region Queries
        public DbVersionInfo GetDBVersion()
        {
            DbVersionInfo info = _context.DbVersions
                                .Select(x => new DbVersionInfo()
                                {
                                    Major = x.Major,
                                    Minor = x.Minor,
                                    Build = x.Build,
                                    ReleaseDate = x.ReleaseDate
                                }).First();
            return info;
        }
        #endregion

    }
}
