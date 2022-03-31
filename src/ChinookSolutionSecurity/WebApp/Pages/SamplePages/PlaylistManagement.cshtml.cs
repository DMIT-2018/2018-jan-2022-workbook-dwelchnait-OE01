using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.ViewModels;
using WebApp.Helpers;
using ChinookSystem.BLL;
using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using AppSecurity.BLL;
#endregion


namespace WebApp.Pages.SamplePages
{

    public class PlaylistManagementModel : PageModel
    {
        #region Private variables and DI constructor
        private readonly TrackServices _trackServices;
        private readonly PlaylistTrackServices _playlisttrackServices;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SecurityService _Security;

        public PlaylistManagementModel(TrackServices trackservices,
                                PlaylistTrackServices _playlisttrackservices,
                                UserManager<ApplicationUser> userManager, 
                                SecurityService security)
        {
            _trackServices = trackservices;
            _playlisttrackServices = _playlisttrackservices;
            _UserManager = userManager;
            _Security = security;
        }
        #endregion

        #region Messaging and Error Handling
        [TempData]
        public string FeedBackMessage { get; set; }

        public string ErrorMessage { get; set; }

        //a get property that returns the result of the lamda action
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBackMessage);

        //used to display any collection of errors on web page
        public List<string> ErrorDetails { get; set; } = new();

        //PageModel local error list for collection 
        public List<Exception> Errors { get; set; } = new();

        #endregion

        #region Paginator
        private const int PAGE_SIZE = 5;
        public Paginator Pager { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? currentpage { get; set; }
        #endregion

        [BindProperty(SupportsGet = true)]
        public string searchBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string searchArg { get; set; }

        [BindProperty(SupportsGet = true)]
        public string playlistname { get; set; }

        public List<TrackSelection> trackInfo { get; set; }

        //CQRS query data model
        public List<PlaylistTrackInfo> qplaylistInfo { get; set; }

        //CQRS command data model
        [BindProperty]
        public List<PlaylistTrackMove> cplaylistInfo { get; set; }

        [BindProperty]
        public int addtrackid { get; set; }

        #region Security
        public ApplicationUser AppUser { get; set; }
        public string EmployeeName { get; set; }
        public const string USERNAME = "HansenB"; //pretend user until security implemented
        #endregion

        public async Task OnGet()
        {
            AppUser = await _UserManager.FindByNameAsync(User.Identity.Name);
            EmployeeName = _Security.GetEmployeeName(AppUser.EmployeeId.Value);
            GetTrackInfo();
            GetPlaylist();
        }

        public void GetTrackInfo()
        {
            if (!string.IsNullOrWhiteSpace(searchArg) &&
                            !string.IsNullOrWhiteSpace(searchBy))
            {
                int totalcount = 0;
                int pagenumber = currentpage.HasValue ? currentpage.Value : 1;
                PageState current = new(pagenumber, PAGE_SIZE);
                trackInfo = _trackServices.Track_Fetch_TracksBy(searchArg.Trim(),
                    searchBy.Trim(), pagenumber, PAGE_SIZE, out totalcount);
                Pager = new(totalcount, current);
            }
        }

        public void GetPlaylist()
        {
            if (!string.IsNullOrWhiteSpace(playlistname))
            {
                string username = USERNAME;
                qplaylistInfo = _playlisttrackServices.PlaylistTrack_Fetch_Playlist(playlistname.Trim(), username);
            }
        }
        public IActionResult OnPostTrackSearch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchBy))
                {
                    Errors.Add(new Exception("Track search type not selected"));
                }
                if (string.IsNullOrWhiteSpace(searchArg))
                {
                    Errors.Add(new Exception("Track search string not entered"));
                }
                if (Errors.Any())
                {
                    throw new AggregateException(Errors);
                }
                return RedirectToPage(new
                {
                    searchBy = searchBy.Trim(),
                    searchArg = searchArg.Trim(),
                    playlistname = string.IsNullOrWhiteSpace(playlistname) ? " " : playlistname.Trim()
                });
            }
            catch (AggregateException ex)
            {
                ErrorMessage = "Unable to process search";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();
            }
        }

        public IActionResult OnPostFetch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("Enter a playlist name to fetch.");
                }
                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname.Trim()
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();

            }
        }

        public IActionResult OnPostAddTrack()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistname))
                {
                    throw new Exception("You need to have a playlist select first. Enter a playlist name and Fetch");
                }

                // Add the code to add a track via the service.
                string username = USERNAME; //this will be change when security is implemented
                _playlisttrackServices.PlaylistTrack_AddTrack(playlistname.Trim(),
                    username, addtrackid);
                FeedBackMessage = "adding the track";



                return RedirectToPage(new
                {
                    searchby = searchBy,
                    searcharg = searchArg,
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {

                ErrorMessage = "Unable to process add track";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }

        }

        public IActionResult OnPostRemove()
        {
            try
            {
                //Add the code to process the list of tracks via the service.
                string username = USERNAME;
                _playlisttrackServices.PlaylistTrack_RemoveTracks(playlistname.Trim(),
                    USERNAME, cplaylistInfo);
                FeedBackMessage="Tracks have been removed from your playlist";

                return RedirectToPage(new
                {
                    searchBy = string.IsNullOrWhiteSpace(searchBy) ? " " : searchBy.Trim(),
                    searchArg = string.IsNullOrWhiteSpace(searchArg) ? " " : searchArg.Trim(),
                    playlistname = playlistname
                });
            }
            catch (AggregateException ex)
            {

                ErrorMessage = "Unable to process remove tracks";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                GetTrackInfo();
                GetPlaylist();

                return Page();
            }

        }

        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
    }
}
