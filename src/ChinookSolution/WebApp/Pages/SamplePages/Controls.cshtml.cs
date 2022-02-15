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

        [BindProperty]
        public int MyRide { get; set; }
        //pretend the collection is coming from the database
        //each row of the collection has two values: selection value; selection text
        //the class SelectionList will be use as the datatype for the collection
        //the will need to be recreate on each internet trip
        public List<SelectionList> Rides { get; set; }

        [BindProperty]
        public string VacationSpot { get; set; }
        //pretend the collection is coming from the database
        //create a List<string> to be used on the <datalist>
        public List<string> VacationSpots { get; set; }

        [BindProperty]
        // input type=range
        public int ReviewRating { get; set; }
        public void OnGet()
        {
            PopulateLists();
        }

        public void PopulateLists()
        {
            //used for the drop down list
            Rides = new List<SelectionList>();
            Rides.Add(new SelectionList() {ValueId=1, DisplayText="Car" });
            Rides.Add(new SelectionList() { ValueId=2, DisplayText="Bus" });
            Rides.Add(new SelectionList() { ValueId=3, DisplayText="Bike" });
            Rides.Add(new SelectionList() { ValueId=4, DisplayText="Motorcycle" });
            Rides.Add(new SelectionList() { ValueId=5, DisplayText="Board" });
            Rides.Sort((x,y) => x.DisplayText.CompareTo(y.DisplayText)); //ascending
            //Rides.Sort((x, y) => y.DisplayText.CompareTo(x.DisplayText)); //descending
        
            //used for datalist control
            VacationSpots = new List<string>();
            VacationSpots.Add("California");
            VacationSpots.Add("Caribbean");
            VacationSpots.Add("Cruising");
            VacationSpots.Add("Europe");
            VacationSpots.Add("Florida");
            VacationSpots.Add("Mexico");

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

        public IActionResult OnPostListSlider()
        {
            //echo back the input values
            Feedback = $"Ride {MyRide}; Vacation Spot {VacationSpot}; Control Review Rating {ReviewRating}";
            //on each post, reload the lists
            PopulateLists();
            return Page();
        }

    }

    public class SelectionList
    {
        //this class is designed for a <select> tag (dropdownlist) which needs a
        //value item and a display item
        public int ValueId { get; set; } //will be the returned value
        public string DisplayText { get; set; } //will be the text display
    }
}
