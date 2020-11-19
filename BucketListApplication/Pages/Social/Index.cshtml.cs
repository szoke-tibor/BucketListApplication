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
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				CurrentFilter = searchString;

				var usersQuery = from u in _context.Users
								 where u.Id != CurrentUserId
								 select u;

				if (!String.IsNullOrEmpty(searchString))
					usersQuery = usersQuery.Where(u => u.FullName.Contains(searchString));
				else
					usersQuery = usersQuery.Where(u => u.SeededUser == true);

				Users = await usersQuery.AsNoTracking().ToListAsync();
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}
    }
}
