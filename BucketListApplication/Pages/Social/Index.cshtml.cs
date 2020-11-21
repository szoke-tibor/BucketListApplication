using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.Social
{
    public class IndexModel : PageModel
    {
        private readonly BLContext _context;

        public IndexModel(BLContext context)
        {
            _context = context;
        }

		public string CurrentFilter { get; set; }
		public IList<BLUser> Users { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchString)
        {
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId != null)
			{
				CurrentFilter = searchString;

				if (!String.IsNullOrEmpty(searchString))
					Users = await _context.Users
						.AsNoTracking()
						.Where(u => u.FullName.Contains(searchString))
						.Where(u => u.Id != currentUserId)
						.ToListAsync();
				else
					Users = await _context.Users
						.AsNoTracking()
						.Where(u => u.SeededUser == true)
						.Where(u => u.Id != currentUserId)
						.ToListAsync();

				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}
    }
}
