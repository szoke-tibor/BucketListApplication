using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;

namespace BucketListApplication.Pages.Elements
{
    public class EditModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public EditModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Element Element { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Element = await _context.Elements.FindAsync(id);

            if (Element == null)
            {
                return NotFound();
            }
            return Page();
        }

		// DONE
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
			var elementToUpdate = await _context.Elements.FindAsync(id);

			if (elementToUpdate == null)
			{
				return NotFound();
			}

			if (await TryUpdateModelAsync<Element>(
				elementToUpdate,
				"element",
				e => e.Name))
			{
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

			return Page();
		}

        private bool ElementExists(int id)
        {
            return _context.Elements.Any(e => e.ElementID == id);
        }
    }
}
