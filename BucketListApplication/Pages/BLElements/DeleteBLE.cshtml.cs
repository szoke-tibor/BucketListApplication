using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BLElements
{
    public class DeleteModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;

        public DeleteModel(BLContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? bucketListElementId, bool? saveChangesError = false)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListElementId == null)
                return NotFound();

            BucketListElement = await _context.BLElements
                .AsNoTracking()
                .Include(ble => ble.BucketList)
				.FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);

            if (BucketListElement == null)
                return NotFound();

            if (_userService.BucketListElementIsNotBelongingToUser(User, BucketListElement))
                return Forbid();

            if (saveChangesError.GetValueOrDefault())
				ErrorMessage = "Delete failed. Try again";

			return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListElementId)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListElementId == null)
                return NotFound();

            var BLElementToRemove = await _context.BLElements
                                    .Include(ble => ble.BucketList)
                                    .FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);

            if (BLElementToRemove == null)
				return NotFound();

            if (_userService.BucketListElementIsNotBelongingToUser(User, BLElementToRemove))
                return Forbid();

            try
			{
				_context.BLElements.Remove(BLElementToRemove);
				await _context.SaveChangesAsync();
                return RedirectToPage("../BucketLists/Index", new { bucketListId = BLElementToRemove.BucketListID });
            }
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction("DeleteBLE", new { bucketListElementId, saveChangesError = true });
			}
        }
    }
}
