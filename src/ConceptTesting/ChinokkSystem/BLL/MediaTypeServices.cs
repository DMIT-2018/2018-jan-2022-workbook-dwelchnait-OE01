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
    public class MediaTypeServices
    {
        #region Constructor and Dependencies
        private readonly ChinookContext _context;
        internal MediaTypeServices(ChinookContext context)
        {
            this._context = context;
        }
        #endregion

        public List<KeyValueOption<int>> ListMediaTypeNames()
        {
            //using the KeyValueOption class
            //requires the code to indicate the type of key value <T>
            //    which in this case is an integer
            return _context.MediaTypes
                .Select(x => new KeyValueOption<int>
                {
                    Key = x.MediaTypeId,
                    DisplayText = x.Name
                }).ToList();
        }

    }
}
