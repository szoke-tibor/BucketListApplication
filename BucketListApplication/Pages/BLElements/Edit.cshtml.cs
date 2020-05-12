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

//Not yet implemented
namespace BucketListApplication.Pages.BLElements
{
    public class EditModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public EditModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketListElement BLElement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BLElement = await _context.BLElements.FindAsync(id);

            if (BLElement == null)
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
			var elementToUpdate = await _context.BLElements.FindAsync(id);

			if (elementToUpdate == null)
			{
				return NotFound();
			}

			if (await TryUpdateModelAsync<BucketListElement>(
				elementToUpdate,
				"blelement",
				ble => ble.Name))
			{
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

			return Page();
		}

        private bool BLElementExists(int id)
        {
            return _context.BLElements.Any(e => e.ElementID == id);
        }
    }
}
