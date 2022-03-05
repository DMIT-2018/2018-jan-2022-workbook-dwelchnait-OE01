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
    public class GenreServices
    {

            #region Constructor and Dependencies
            private readonly ChinookContext _context;
            internal GenreServices(ChinookContext context)
            {
                this._context = context;
            }
        #endregion

        public List<KeyValueOption<int>> ListGenreNamesForMedia(int mediatypeid)
        {
            //obtain a distinct list of Genres for the supplied mediatype
            //using the KeyValueOption class
            //requires the code to indicate the type of key value <T>
            //      which in this case is an integer
            //Tracks GenreId is a nullable int, select only those tracks with
            //      a non-nullable GenreId
            return _context.Tracks
                    .Where(t => t.MediaTypeId == mediatypeid 
                        && t.GenreId.HasValue)
                    .Select(t => new KeyValueOption<int>
                    {
                        Key = (int)t.GenreId,
                        DisplayText = t.Genre.Name
                    })
                      .Distinct()
                      .OrderBy(t => t.DisplayText).ToList();
        }


    }
}
