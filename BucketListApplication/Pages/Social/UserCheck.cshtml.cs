using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using BucketListApplication.Pages.BLElements;

namespace BucketListApplication.Pages.Social
{
    public class UserCheckModel : BLElementListingPageModel
	{
        private readonly BucketListApplication.Data.BLContext _context;

        public UserCheckModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public string Title { get; set; }

		public async Task<IActionResult> OnGetAsync(string Id)
        {
			//Logged user's userId
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				Title = _context.Users.FirstOrDefault(u => u.Id == Id).FullName + " Bakancslistái";
				PopulateBucketListDropDownList(_context, Id);
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}

		public async Task<IActionResult> OnPostAsync(string Id)
		{
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				PopulateBucketListDropDownList(_context, Id);
				PopulateSelectedBLElementsList(_context, SelectedBucketList.BucketListID, true);
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}
	}
}
