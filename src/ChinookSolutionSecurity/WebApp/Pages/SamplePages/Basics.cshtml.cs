using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.SamplePages
{
    public class BasicsModel : PageModel
    {
        //basiclly this is an object, treat it as such

        //data fields
        public string MyName;

        //properties

        //the annotation [TempData] stores data until it's read in another
        //  immediate request
        //this annotation has two methods called Keep(string) and
        //  Peek(string) (used on Content page)
        //kept in a dictionary (name/value pairs)
        //useful to redirect when data is required for more than a single request
        //Implemented by providers using either cookies or session state
        //TempData os NOT bound to any pariticular control like BindProperty

        [TempData]
        public string Feedback { get; set; }

        //annotation of [BindProperty], ties a property in the PageModel class
        //  directly to a control on the Content Page
        //there is a one to one association
        //data is transferred between the two, automatically
        //on the Content page, the control to use this property will have
        //  a helper-tag called asp-for

        //to retain the value in the control, tied to this property AND retained
        //  via the @page use the SupportsGet attribute = true;
        [BindProperty(SupportsGet = true)]
        public int? id { get; set; }

        //constructors

        //behaviours (aka methods)
        public void OnGet()
        {
            //executes in response to a Get request from the browser
            //when the page is "first" accessed, the browser issues a Get request
            //when the page is refreshed, WITHOUT a POST, the browser issues a Get request
            //when the page is processed in response to a form's POST request and using 
            //      RedirectToPage() in the response logic, a Get request will be also issued
            //      after the completion of the POST
            //IF NO RedirectToPage() is used on the POST, there is NO Get request issued

            //create some logic to display to the page
            Random rnd = new Random();
            int oddeven = rnd.Next(0,25);
            if(oddeven % 2 == 0)
            {
                MyName = $"Don is even {oddeven}";
            }
            else
            {
                MyName = null;
            }
        }

        //processing in response to a request from a form on a web page
        // is normally referred to as a Post (method="post")

        //General Post (OnPost())
        //a general post occurs when an asp-page-handler is NOT used
        //the return datatype can be void, however, you will normally
        //  encounter the datatype IActionResult
        //the IActionResult requires some type of request action
        //  on the return statement of the method OnPost()
        //typical actions:
        //  Page()
        //   :does NOT issue aOnGet request
        //   :remains on the current page
        //   :a good action for form processing involving validation
        //      and with the catch of a try/catch
        //  RedirectToPage()
        //  :DOES issue a OnGet request
        //  :is used to retain input value via the @page and your BindProperty
        //      for controls on your form on the Content page

        public IActionResult OnPost()
        {
            //this line of code is used to cause a delay in processing
            //  so we can see on the Network Activity some type of
            //  simulated processing
            Thread.Sleep(2000);

            //retreive data via the Request object
            //  Request: web page to server
            //  Response: server to web page
            string buttonvalue = Request.Form["theButton"];
            Feedback = $"Button pressed is {buttonvalue} with numberic input of {id}";
            //return Page(); //does NOT issue a OnGet() request
            return RedirectToPage(new { id = id }); //issues a request for OnGet()
        }
    }
}
