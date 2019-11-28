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
    public class DeleteModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public DeleteModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Element Element { get; set; }
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Element = await _context.Elements
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.ElementID == id);

            if (Element == null)
            {
                return NotFound();
            }

			if (saveChangesError.GetValueOrDefault())
			{
				ErrorMessage = "Delete failed. Try again";
			}

			return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var element = await _context.Elements.FindAsync(id);

			if (element == null)
			{
				return NotFound();
			}

			try
			{
				_context.Elements.Remove(element);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction("./Delete",
									 new { id, saveChangesError = true });
			}
        }
    }
}
