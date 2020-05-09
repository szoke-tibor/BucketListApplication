using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;

//Not yet implemented
namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public CreateModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
			//Logged user's userId
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				//Logged user's BucketLists
				var CurrentUsersBucketLists = from bl in _context.BucketLists
											  where bl.UserId == CurrentUserId
											  select bl;
				ViewData["BucketList"] = new SelectList(CurrentUsersBucketLists, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name));
			}
			else
				throw new Exception("Nincs bejelentkezett felhasználó.");

			ViewData["Design"] = new SelectList(_context.Designs, nameof(Models.Design.DesignID), nameof(Models.Design.Name));
            return Page();
        }

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

		// DONE
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			var emptyBucketListElement = new BucketListElement();

			if (await TryUpdateModelAsync<BucketListElement>(
				emptyBucketListElement,
				"bucketListElement",   // Prefix for form value.
				ble => ble.Name, ble => ble.DesignID,
                ble => ble.BucketListID, ble => ble.Description, ble => ble.Completed, ble => ble.Visibility))
			{
				_context.Elements.Add(emptyBucketListElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("../Elements/Index");
			}
			return Page();
		}
    }
}
