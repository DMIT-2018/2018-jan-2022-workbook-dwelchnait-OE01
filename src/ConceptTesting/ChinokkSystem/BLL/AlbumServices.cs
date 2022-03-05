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
    #nullable disable
    public class AlbumServices
    {
        #region Constructor and Dependencies
        private readonly ChinookContext _context;
        internal AlbumServices(ChinookContext context)
        {
            this._context = context;
        }
        #endregion

        public List<AlbumItem> AlbumsForMediaTypeGenre(int mediatypeid, int genreid,
                                               int pagenumber,
                                               int pagesize,
                                               out int totalcount)
        {
            IEnumerable<AlbumItem> info = _context.Tracks
                                            .Where(t => t.GenreId == genreid
                                                && t.MediaTypeId == mediatypeid
                                                && t.AlbumId.HasValue)
                                            .Select(t => new AlbumItem
                                            {
                                                AlbumId = (int)t.AlbumId,
                                                Title = t.Album.Title,
                                                ArtistId = t.Album.ArtistId,
                                                ReleaseYear = t.Album.ReleaseYear,
                                                ReleaseLabel = t.Album.ReleaseLabel
                                            })
                                            .Distinct()
                                            .OrderBy(a => a.Title);
            totalcount = info.Count();
            int skipRows = (pagenumber - 1) * pagesize;
            return info.Skip(skipRows).Take(pagesize).ToList();
        }
    }
}
