#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
#endregion


namespace WebApp.Pages.SamplePages
{
    public class AlbumsByGenreQueryModel : PageModel
    {
        #region Private variable and DI constructor
        private readonly ILogger<AlbumsByGenreQueryModel> _logger;
        private readonly AlbumServices _albumServices;
        private readonly GenreServices _genreServices;

        public AlbumsByGenreQueryModel(ILogger<AlbumsByGenreQueryModel> logger,
                                        AlbumServices albumservices,
                                        GenreServices genreservices)
        {
            _logger = logger;
            _albumServices = albumservices;
            _genreServices = genreservices;
        }
        #endregion

        #region FeedBack and ErrorHandling
        [TempData]
        public string FeedBack { get; set; }
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBack);

        [TempData]
        public string ErrorMsg { get; set; }
        public bool HasErrorMsg => !string.IsNullOrWhiteSpace(ErrorMsg);

        #endregion

        [BindProperty]
        public List<SelectionList> GenreList { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        public void OnGet()
        {
            //consume a service GetAllGenres()
            GenreList = _genreServices.GetAllGenres();
            //the presentation layer would like to order the list
            //use the .Sort() method of the List<T> class
            GenreList.Sort((x,y) => x.DisplayText.CompareTo(y.DisplayText));
        }

        public IActionResult OnPost() //result of pushing a button on a form with method="post"
        {
            if (GenreId == 0)
            {
                FeedBack = "You did not select a genre";
            }
            else
            {
                FeedBack = $"You selected the genre id {GenreId}";
            }
            return RedirectToPage(); //cause a Get request to be issued; cause OnGet to execute
        }
    }
}
