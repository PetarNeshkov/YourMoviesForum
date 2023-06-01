using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.Users;

namespace YourMovies.Web.Areas.Identity.Pages.Account;

public class SuccessfulRemoval : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IUserService userService;

    public SuccessfulRemoval(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserService userService)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.userService = userService;
    }
    
    public async Task<IActionResult> OnGetAsync(string email, string username)
    {
        if (email == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await userManager.FindByEmailAsync(email);
        

        var result = await userManager.DeleteAsync(user);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Unexpected error occurred deleting user.");
        }
        
        await signInManager.SignOutAsync();

        return Page();
    }
}