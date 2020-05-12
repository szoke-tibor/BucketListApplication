using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;

//Not yet implemented
namespace BucketListApplication.Pages.BLElements
{
    public class DetailsModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public DetailsModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public BucketListElement BLElement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Element = await _context.Elements.FirstOrDefaultAsync(m => m.ID == id);

            BLElement = await _context.BLElements
                .Include(e => e.ElementCategories)
				.ThenInclude(ec => ec.Category)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.ElementID == id);

			if (BLElement == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
