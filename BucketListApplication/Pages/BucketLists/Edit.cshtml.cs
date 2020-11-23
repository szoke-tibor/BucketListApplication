using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BucketLists
{
    public class EditModel : PageModel
    {
        private readonly BLContext _context;

        public EditModel(BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? bucketListId)
        {
            //Logged user's userId
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
            {
                if (bucketListId == null)
                    return NotFound();

                BucketList = await _context.BucketLists.FindAsync(bucketListId);

                if (BucketList == null)
                    return NotFound();

                //Not the owner tries to edit their BucketListElement
                if (BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                    return Forbid();

                return Page();
            }
            else
                return RedirectToPage("../AuthError");
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListId)
        {
            if (bucketListId == null)
                return NotFound();

            var bucketlistToUpdate = await _context.BucketLists.FindAsync(bucketListId);

            if (bucketlistToUpdate == null)
                return NotFound();

            //Not the owner tries to edit their BucketListElement
            if (bucketlistToUpdate.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            // Defense against overposting attacks. Returns true if the update was successful.
            if (await TryUpdateModelAsync<BucketList>(bucketlistToUpdate, "BucketList",
                bl => bl.Name,
                bl => bl.Visibility))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("../BucketLists/Index", new { bucketListId = bucketlistToUpdate.BucketListID });
            }
            return Page();
        }
    }
}
