
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
using WebApp.Helpers;
#endregion

namespace WebApp.Pages
{
    #nullable disable
    public class FilterSearchModel : PageModel
    {
        #region Private variables and DI constructor
        private readonly AlbumServices _albumServices;
        private readonly GenreServices _genreServices;
        private readonly MediaTypeServices _mediatypeServices;


        public FilterSearchModel(AlbumServices albumservices,
                                GenreServices genreservices,
                                MediaTypeServices mediatypeservices)
        {
            _albumServices = albumservices;
            _genreServices = genreservices;
            _mediatypeServices = mediatypeservices;


        }

        [TempData]
        public string FeedBackMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        //a get property that returns the result of the lamda action
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBackMessage);
        #endregion

        [BindProperty(SupportsGet =true)]
        public int? MediaTypeId { get; set; }

        public List<KeyValueOption<int>> MediaList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }

        public List<KeyValueOption<int>> GenreList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? AlbumId { get; set; }

        public List<AlbumItem> AlbumList { get; set; } = new();


        #region Paginator
        private const int PAGE_SIZE = 5;
        public Paginator Pager { get; set; }
        #endregion

        public void OnGet(int? currentpage)
        {
            PopulateForm(currentpage);
        }

        private void PopulateForm(int? currentpage)
        {
            MediaList = _mediatypeServices.ListMediaTypeNames();
            if (MediaTypeId.HasValue)
            {
                GenreList = _genreServices.ListGenreNamesForMedia((int)MediaTypeId.Value);
                if(GenreId.HasValue)
                {
                   int pageNumber = currentpage.HasValue ? currentpage.Value : 1;
                    PageState currrent = new(pageNumber, PAGE_SIZE);
                    int totalcount;
                    AlbumList = _albumServices.AlbumsForMediaTypeGenre((int)MediaTypeId.Value,
                                                               (int)GenreId.Value,
                                                               pageNumber,
                                                               PAGE_SIZE,
                                                               out totalcount);
                    Pager = new(totalcount, currrent);
                   
                }
            }
        }

        public IActionResult OnPostMediaType()
        {
            
            return RedirectToPage(new { MediaTypeId = MediaTypeId, 
                                        GenreId = (int?)null, 
                                        AlbumId = (int?)null });

        }
        public IActionResult OnPostGenre()
        {

            return RedirectToPage(new
            {
                MediaTypeId = MediaTypeId,
                GenreId = GenreId,
                AlbumId = (int?)null
            });

        }

        public IActionResult OnPostAlbum()
        {
            FeedBackMessage = "Your choices can be seen in the url values.";
            return RedirectToPage(new
            {
                MediaTypeId = MediaTypeId,
                GenreId = GenreId,
                AlbumId = AlbumId
            });
        }
        public IActionResult OnPostClear()
        {
            MediaTypeId = null;
            GenreId = null;
            AlbumId = null;
            return RedirectToPage(new
            {
                MediaTypeId = (int?)null,
                GenreId = (int?)null,
                AlbumId = (int?)null
            });
        }
    }
}
