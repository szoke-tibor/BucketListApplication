using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BucketLists
{
    public class DeleteModel : PageModel
    {
        private readonly BLContext _context;

        public DeleteModel(BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public string ErrorMessage { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? bucketListId, bool? saveChangesError = false)
        {
            if (bucketListId == null)
                return NotFound();

            BucketList = await _context.BucketLists
                         .AsNoTracking()
                         .Include(bl => bl.BLElements)
                         .FirstOrDefaultAsync(bl => bl.BucketListID == bucketListId);

            if (BucketList == null)
                return NotFound();

            //Not the owner tries to delete their BucketList
            if (BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            if (saveChangesError.GetValueOrDefault())
                ErrorMessage = "Delete failed. Try again";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListId)
        {
            if (bucketListId == null)
                return NotFound();

            var BucketListToRemove = await _context.BucketLists.FindAsync(bucketListId);

            if (BucketListToRemove == null)
                return NotFound();

            //Not the owner tries to delete their BucketList
            if (BucketListToRemove.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            try
            {
                _context.BucketLists.Remove(BucketListToRemove);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteBL", new { bucketListId, saveChangesError = true });
            }
        }
    }
}