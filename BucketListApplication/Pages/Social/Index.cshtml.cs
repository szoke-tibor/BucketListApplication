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
using System.Runtime.CompilerServices;

namespace BucketListApplication.Pages.Social
{
    public class IndexModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public IndexModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

		public string CurrentFilter { get; set; }
		public IList<BLUser> Users { get; set; }

        public async Task OnGetAsync(string searchString)
        {
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				CurrentFilter = searchString;

				var usersQuery = from u in _context.Users
								 where u.Id != CurrentUserId
								 select u;

				if (!String.IsNullOrEmpty(searchString))
					usersQuery = usersQuery.Where(u => u.FullName.Contains(searchString));

				Users = await usersQuery.AsNoTracking().ToListAsync();
			}
			else
				RedirectToPage("../Index");
		}
    }
}
