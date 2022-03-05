using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
#endregion

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AboutServices _aboutService;


        public IndexModel(ILogger<IndexModel> logger,
                            AboutServices aboutservices)
        {
            _logger = logger;
            _aboutService = aboutservices;
        }

        public DbVersionInfo info { get; set; }
        public void OnGet()
        {
            info = _aboutService.GetDBVersion();
        }
    }
}