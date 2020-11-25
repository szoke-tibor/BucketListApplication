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
    public class DetailsModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;
        private readonly IBucketListService _bucketListService;

        public DetailsModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
            _userService = userService;
            _bucketListService = bucketListService;
        }

        public BucketListElement BucketListElement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? bucketListElementId)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListElementId == null)
                return NotFound();

            BucketListElement = await _bucketListService.GetBLEByID_WithBLETasksAndCategoryAsync(_context, bucketListElementId);

			if (BucketListElement == null)
                return NotFound();

            if (_userService.BucketListElementIsNotBelongingToUser(User, BucketListElement))
                return Forbid();

            return Page();
        }
    }
}
