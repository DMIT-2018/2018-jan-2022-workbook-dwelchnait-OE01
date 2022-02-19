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
    public  class GenreServices
    {
        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        //obtan the context link from IServiceCollection when this
        //  set of services is injected into the "outside user"
        internal GenreServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Services : Queries
        public List<SelectionList> GetAllGenres()
        {
            //obtain a list of Genres to be used in a select list
            IEnumerable<SelectionList> info = _context.Genres
                                                .Select(g => new SelectionList 
                                                {
                                                    ValueId = g.GenreId,
                                                    DisplayText = g.Name
                                                });
                                            //  .OrderBy(x => x.DisplayText); this sort is in SQL
            return info.ToList();
            //return info.OrderBy(x => x.DisplayText).ToList(); this sort is in RAM
        }
        #endregion
    }
}
