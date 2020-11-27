using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BucketLists
{
    public class DeleteModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;
        private readonly IBucketListService _bucketListService;

        public DeleteModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
            _userService = userService;
            _bucketListService = bucketListService;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public string ErrorMessage { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? bucketListId, bool? saveChangesError = false)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListId == null)
                return NotFound();

            BucketList = await _bucketListService.GetBLByIDWithBLEsAsync(_context, bucketListId);

            if (BucketList == null)
                return NotFound();

            if (_userService.BucketListIsNotBelongingToUser(User, BucketList))
                return Forbid();

            if (saveChangesError.GetValueOrDefault())
                ErrorMessage = "Delete failed. Try again";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListId)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListId == null)
                return NotFound();

            var bucketListToRemove = await _bucketListService.GetBLByIDAsync(_context, bucketListId);

            if (bucketListToRemove == null)
                return NotFound();

            if (_userService.BucketListIsNotBelongingToUser(User, bucketListToRemove))
                return Forbid();

            try
            {
                _context.BucketLists.Remove(bucketListToRemove);
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