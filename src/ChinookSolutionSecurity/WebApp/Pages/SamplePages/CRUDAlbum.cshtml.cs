#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
#endregion


namespace WebApp.Pages.SamplePages
{
    public class CRUDAlbumModel : PageModel
    {
        #region Service variable(s), FeedBack and DI constructor
        private readonly AlbumServices _albumservices;
        private readonly ArtistServices _artistservices;

        [TempData]
        public string FeedBackMessage { get; set; }

        public string ErrorMessage { get; set; }

        public CRUDAlbumModel(AlbumServices albumservices,
                              ArtistServices artistservices)
        {
            _albumservices = albumservices;
            _artistservices = artistservices;
        }
        #endregion

        #region Form Variables
        [BindProperty(SupportsGet = true)]
        public int? albumid { get; set; }

        [BindProperty]
        public List<SelectionList> Artists { get; set; }

        [BindProperty]
        public AlbumItem Album { get; set; }
        #endregion

        public void OnGet()
        {
            Artists = _artistservices.Artists_List();
            if (albumid.HasValue && albumid > 0)
            {
                Album = _albumservices.Albums_GetAlbumById((int)albumid);
            }
        }

        public IActionResult OnPostNew()
        {
            try
            {
                //if you did not directly place the select value of your drop down list
                //  into your crud instance the you would have to manually move the value
                //  into your crud instance
                albumid = _albumservices.AddAlbum(Album);
                FeedBackMessage = $"Album ({albumid}) has been added";
                //the response to the browser  is a Post Redirect Get pattern 
                return RedirectToPage(new { albumid = albumid });
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                Artists = _artistservices.Artists_List();
                //the response to the browser is the result of Post processing
                //this means the OnGet() will not be executed
                return Page();
            }

        }


        public IActionResult OnPostUpdate()
        {
            try
            {
                if (albumid.HasValue)
                {
                    int rowaffected = _albumservices.UpdateAlbum(Album);
                    if (rowaffected > 0)
                    {
                        FeedBackMessage = "Album has been updated";
                    }
                    else
                    {
                        FeedBackMessage = "No album update. Album does not exist";
                    }
                }
                else
                {
                    FeedBackMessage = "Find an album to maintain before attempting the update";
                }
                //the response to the browser  is a Post Redirect Get pattern 
                return RedirectToPage(new { albumid = albumid });
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                Artists = _artistservices.Artists_List();
                //the response to the browser is the result of Post processing
                //this means the OnGet() will not be executed
                return Page();
            }

        }
        public IActionResult OnPostDelete()
        {
            try
            {
                if (albumid.HasValue)
                {
                    int rowaffected = _albumservices.DeleteAlbum(Album);
                    if (rowaffected > 0)
                    {
                        FeedBackMessage = "Album has been removed";
                    }
                    else
                    {
                        FeedBackMessage = "No album remove. Album does not exist";
                    }
                    //remove your pkey value you are hanging on to for Post Get Redirect
                    albumid = null;
                }
                else
                {
                    FeedBackMessage = "Find an album to review before attempting the delete";
                }
                return RedirectToPage(new { albumid = albumid });
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                Artists = _artistservices.Artists_List();
                return Page();
            }

        }

        public IActionResult OnPostSearch()
        {
            return RedirectToPage("/SamplePages/AlbumsByGenreQuery");
        }

        // this method will drill down into Exceptions to find the InnerException
        // often you may get an error message referring you to the InnerException
        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                //promote the InnerException to be the Exception
                //this loop will continue until to get to the most InnerException
                //      which is your actual error you wish to see
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}
