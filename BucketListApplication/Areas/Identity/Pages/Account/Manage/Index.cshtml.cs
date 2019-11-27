using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BucketListApplication.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<BLUser> _userManager;
        private readonly SignInManager<BLUser> _signInManager;

        public IndexModel(
            UserManager<BLUser> userManager,
            SignInManager<BLUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
			[Required]
			[DataType(DataType.Text)]
			[Display(Name = "Full name")]
			public string FullName { get; set; }

			[DataType(DataType.Text)]
			public string About { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "Profile Image URL")]
			public string ProfileImageURL { get; set; }
        }

        private async Task LoadAsync(BLUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
				FullName = user.FullName,
				About = user.About,
				ProfileImageURL = user.ProfileImageURL,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

			if (Input.FullName != user.FullName)
			{
				user.FullName = Input.FullName;
			}

			if (Input.About != user.About)
			{
				user.About = Input.About;
			}

			if (Input.ProfileImageURL != user.ProfileImageURL)
			{
				user.ProfileImageURL = Input.ProfileImageURL;
			}

			await _userManager.UpdateAsync(user);

			await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
