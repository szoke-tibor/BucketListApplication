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
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BucketListApplication.Pages.BLElements
{
    public class IndexModel : BLElementListingPageModel
	{
        private readonly BucketListApplication.Data.BLContext _context;

        public IndexModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public async Task OnGetAsync()
        {
			//Logged user's userId
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				PopulateBucketListDropDownList(_context, CurrentUserId);
			}
			else
				RedirectToPage("../Index");

		}
		public async Task<IActionResult> OnPostAsync()
		{
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				PopulateBucketListDropDownList(_context, CurrentUserId);
				PopulateSelectedBLElementsList(_context, SelectedBucketList.BucketListID, false);
			}
			else
				return RedirectToPage("../Index");

			return Page();
		}
	}
}
