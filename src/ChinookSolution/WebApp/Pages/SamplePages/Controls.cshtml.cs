using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.SamplePages
{
    public class ControlsModel : PageModel
    {
        [TempData]
        public string Feedback { get; set; }

        [BindProperty]
        public string EmailText { get; set; }

        [BindProperty]
        public string PasswordText { get; set; }

        [BindProperty]
        //public DateTime DateTimeText { get; set; } = DateTime.Today;
        public string DateTimeText { get; set; }

        [BindProperty]
        public string RadioMeal { get; set; }
        public string[] Meals { get; set; } = new[] {"breakfast", "lunch", "dinner/supper", "snacks" };

        [BindProperty]
        public bool AcceptanceBox { get; set; }

        [BindProperty]
        public string MessageText { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostText()
        {
            //echo back the input values
            Feedback = $"Email {EmailText}; Password {PasswordText}; Date {DateTimeText}";
            return Page();
        }

        public IActionResult OnPostRadioCheckArea()
        {
            //echo back the input values
            Feedback = $"Meal {RadioMeal}; Acceptance {AcceptanceBox}; Message {MessageText}";
            return Page();
        }

    }
}
