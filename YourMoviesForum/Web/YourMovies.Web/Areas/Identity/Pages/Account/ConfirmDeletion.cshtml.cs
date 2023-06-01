using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YourMovies.Web.Areas.Identity.Pages.Account;

public class ConfirmDeletion : PageModel
{
    public async Task<IActionResult> OnGet(string email)
    {
        if (email == null)
        {
            return RedirectToPage("/Index");
        }
        
        return Page();
    }
}