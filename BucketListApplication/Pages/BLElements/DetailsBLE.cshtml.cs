using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BLElements
{
    public class DetailsModel : PageModel
    {
        private readonly BLContext _context;

        public DetailsModel(BLContext context)
        {
            _context = context;
        }

        public BucketListElement BucketListElement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? bucketListElementId)
        {
            if (bucketListElementId == null)
                return NotFound();

            BucketListElement = await _context.BLElements
                .AsNoTracking()
                .Include(ble => ble.BucketList)
                .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
				.FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);

			if (BucketListElement == null)
                return NotFound();

            //Not the owner tries to view their BucketListElement
            if (BucketListElement.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return Page();
        }
    }
}
