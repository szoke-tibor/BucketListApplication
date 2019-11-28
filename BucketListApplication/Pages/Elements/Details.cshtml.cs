using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;

namespace BucketListApplication.Pages.Elements
{
    public class DetailsModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public DetailsModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public Element Element { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Element = await _context.Elements.FirstOrDefaultAsync(m => m.ID == id);

			Element = await _context.Elements
				.Include(e => e.ElementCategories)
				.ThenInclude(ec => ec.Category)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.ElementID == id);

			if (Element == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
