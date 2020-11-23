using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BucketListApplication.Pages.BucketLists
{
    public class CreateModel : PageModel
    {
        private readonly BLContext _context;

        public CreateModel(BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("../AuthError");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("../AuthError");

            BucketList.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
