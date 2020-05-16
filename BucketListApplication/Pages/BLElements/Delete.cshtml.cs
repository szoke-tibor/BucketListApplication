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
    public class DeleteModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public DeleteModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketListElement BLElement { get; set; }
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            BLElement = await _context.BLElements
                .AsNoTracking()
				.FirstOrDefaultAsync(m => m.ElementID == id);

            if (BLElement == null)
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

			var blelement = await _context.BLElements.FindAsync(id);

			if (blelement == null)
			{
				return NotFound();
			}

			try
			{
				_context.BLElements.Remove(blelement);
				await _context.SaveChangesAsync();
				return RedirectToPage("Index");
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction("Delete",
									 new { id, saveChangesError = true });
			}
        }
    }
}
