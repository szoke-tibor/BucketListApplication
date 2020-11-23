using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BucketListApplication.Pages.BucketLists
{
    public class CreateModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;

        public CreateModel(BLContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public IActionResult OnGet()
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            BucketList.UserId = _userService.GetUserId(User);

            var emptyBucketList = new BucketList();

            if (await TryUpdateModelAsync<BucketList>(emptyBucketList, "BucketList",
                bl => bl.Name,
                bl => bl.UserId))
            {
                await _context.BucketLists.AddAsync(BucketList);
                await _context.SaveChangesAsync();
                var newBucketList = await _context.BucketLists
                    .AsNoTracking()
                    .Where(bl => bl.Name == BucketList.Name)
                    .Where(bl => bl.UserId == BucketList.UserId)
                    .SingleOrDefaultAsync();
				return RedirectToPage("./Index", new { bucketListId = newBucketList.BucketListID });
			}
            return Page();
        }
    }
}
