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
    }
}
