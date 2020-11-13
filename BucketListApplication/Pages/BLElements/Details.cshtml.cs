using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;

namespace BucketListApplication.Pages.BLElements
{
    public class DetailsModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public DetailsModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public BucketListElement BucketListElement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            BucketListElement = await _context.BLElements
                .Include(ble => ble.BucketList)
                .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
                .AsNoTracking()
				.FirstOrDefaultAsync(ble => ble.ElementID == id);

			if (BucketListElement == null)
                return NotFound();

            //Not the owner tries to view their BucketListElement
            if (BucketListElement.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return Page();
        }
    }
}
