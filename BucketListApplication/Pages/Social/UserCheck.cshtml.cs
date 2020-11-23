﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Interfaces;
using System.Security.Claims;
using BucketListApplication.Pages.BLElements;

namespace BucketListApplication.Pages.Social
{
    public class UserCheckModel : BLElementListingPageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;
		private readonly IBucketListService _bucketListService;

		public UserCheckModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
			_userService = userService;
			_bucketListService = bucketListService;
		}

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public string Title { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var selectedUser = await _context.Users.FindAsync(userId);
			Title = selectedUser.FullName + " Bakancslistái";
			_bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL, true, true);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string userId)
		{
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var selectedUser = await _context.Users.FindAsync(userId);
			Title = selectedUser.FullName + " Bakancslistái";
			_bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL, true, true);
			_bucketListService.PopulateSelectedBLElementsList(_context, SelectedBucketList.BucketListID, true, ref SelectedBLElements);
			return Page();
		}
	}
}
