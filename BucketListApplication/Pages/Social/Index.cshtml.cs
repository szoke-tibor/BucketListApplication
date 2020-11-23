using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.Social
{
    public class IndexModel : PageModel
    {
        private readonly BLContext _context;
		private readonly IUserService _userService;

		public IndexModel(BLContext context, IUserService userService)
        {
            _context = context;
			_userService = userService;
		}

		public string CurrentFilter { get; set; }
		public IList<BLUser> Users { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchString)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			CurrentFilter = searchString;

			if (!String.IsNullOrEmpty(searchString))
				Users = await _context.Users
					.AsNoTracking()
					.Where(u => u.FullName.Contains(searchString))
					.Where(u => u.Id != _userService.GetUserId(User))
					.ToListAsync();
			else
				Users = await _context.Users
					.AsNoTracking()
					.Where(u => u.SeededUser == true)
					.Where(u => u.Id != _userService.GetUserId(User))
					.ToListAsync();

			return Page();
		}
    }
}
