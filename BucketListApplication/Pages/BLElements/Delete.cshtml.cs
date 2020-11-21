using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BLElements
{
    public class DeleteModel : PageModel
    {
        private readonly BLContext _context;

        public DeleteModel(BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            BucketListElement = await _context.BLElements
                .AsNoTracking()
                .Include(ble => ble.BucketList)
				.FirstOrDefaultAsync(ble => ble.ElementID == id);

            if (BucketListElement == null)
                return NotFound();

            //Not the owner tries to delete their BucketListElement
            if (BucketListElement.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            if (saveChangesError.GetValueOrDefault())
				ErrorMessage = "Delete failed. Try again";

			return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var BLElementToRemove = await _context.BLElements
                                    .Include(ble => ble.BucketList)
                                    .FirstOrDefaultAsync(ble => ble.ElementID == id);

            if (BLElementToRemove == null)
				return NotFound();

            //Not the owner tries to delete their BucketListElement
            if (BLElementToRemove.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            try
			{
				_context.BLElements.Remove(BLElementToRemove);
				await _context.SaveChangesAsync();
                return RedirectToPage("../BucketLists/Index", new { selectedbucketlistid = BLElementToRemove.BucketListID });
            }
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction("Delete", new { id, saveChangesError = true });
			}
        }
    }
}
