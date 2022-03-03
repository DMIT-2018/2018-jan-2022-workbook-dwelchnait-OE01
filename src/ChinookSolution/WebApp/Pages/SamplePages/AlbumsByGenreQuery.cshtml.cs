#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
using WebApp.Helpers; //contains the class of the Paginator
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

        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }

        [BindProperty]
        public List<AlbumsListBy> AlbumsByGenre { get; set; }

        #region Paginator variables
        //desired page size
        private const int PAGE_SIZE = 5;
        //instance for the Paginator
        public Paginator Pager { get; set; }
        #endregion
        
        //create a Request parameter called currentPage.
        //this parameter will appear on your url address
        //       url address....?currentPage=n   (n represents the current page value)

        public void OnGet(int? currentPage)
        {
            //consume a service GetAllGenres()
            GenreList = _genreServices.GetAllGenres();
            //the presentation layer would like to order the list
            //use the .Sort() method of the List<T> class
            GenreList.Sort((x,y) => x.DisplayText.CompareTo(y.DisplayText));

            //remember that this method executes as the page FIRST comes up BEFORE
            //      anything has happened on the page (including the FIRST display)
            //any code in this method MUST handle the possibility of missing data for query arguments

            if (GenreId.HasValue && GenreId > 0)
            {
                //installation of the paginator setup
                //determine the page number to use wth the paginator
                int pageNumber = currentPage.HasValue ? currentPage.Value : 1;

                //setup the PageState for use by the Paginator
                PageState current = new PageState(pageNumber, PAGE_SIZE);

                //local variable to hold the total collection size (full row count) of the query
                //this is a variable receive an out value from a method
                int totalrows = 0;

                //for efficiency of data being transferred, we will complete the row
                //  selection in the BLL method
                //this requires the following to be passed into the query method of the BLL:
                //  page number; page size; setup an out variable for the total rows
                AlbumsByGenre = _albumServices.AlbumsByGenre((int)GenreId,
                                                            pageNumber,
                                                            PAGE_SIZE,
                                                            out totalrows);

                //once the query is complete, use the returned total rows AND the PageState
                //   to instaniate an instance of the Paginator
                Pager = new Paginator(totalrows, current);
            }
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
            return RedirectToPage(new {GenreId = GenreId }); //cause a Get request to be issued; cause OnGet to execute
        }
    }
}
