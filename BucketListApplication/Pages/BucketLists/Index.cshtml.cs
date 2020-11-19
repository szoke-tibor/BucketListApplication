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

		public IActionResult OnGet(int? selectedbucketlistid)
        {
			//Logged user's userId
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				PopulateBucketListDropDownList(_context, CurrentUserId, false);
				if (selectedbucketlistid != null)
				{
					SelectedBucketList = _context.BucketLists.Single(bl => bl.BucketListID == selectedbucketlistid);
					PopulateSelectedBLElementsList(_context, (int)selectedbucketlistid, false);
				}
				return Page();
			}
			else
				return RedirectToPage("../AuthError");

		}
		public IActionResult OnPost()
		{
			return RedirectToPage("Index", new { selectedbucketlistid = SelectedBucketList.BucketListID });
		}
	}
}
