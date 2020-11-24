using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BucketLists
{
    public class EditModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;
        private readonly IBucketListService _bucketListService;

        public EditModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
            _userService = userService;
            _bucketListService = bucketListService;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? bucketListId)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListId == null)
                return NotFound();

            BucketList = await _bucketListService.FindBLByID(_context, bucketListId);

            if (BucketList == null)
                return NotFound();

            if (_userService.BucketListIsNotBelongingToUser(User, BucketList))
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListId)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListId == null)
                return NotFound();

            var bucketlistToUpdate = await _bucketListService.FindBLByID(_context, bucketListId);

            if (bucketlistToUpdate == null)
                return NotFound();

            if (_userService.BucketListIsNotBelongingToUser(User, bucketlistToUpdate))
                return Forbid();

            // Defense against overposting attacks. Returns true if the update was successful.
            if (await TryUpdateModelAsync<BucketList>(bucketlistToUpdate, "BucketList",
                bl => bl.Name,
                bl => bl.Visibility))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new { bucketListId = bucketlistToUpdate.BucketListID });
            }
            return Page();
        }
    }
}
