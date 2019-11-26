using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BucketListApplication.Data;
using BucketListApplication.Models;

namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public CreateModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

		// DONE
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			var emptyBucketListElement = new BucketListElement();

			if (await TryUpdateModelAsync<BucketListElement>(
				emptyBucketListElement,
				"bucketListElement",   // Prefix for form value.
				ble => ble.Name, ble => ble.Completed, ble => ble.Description))
			{
				_context.Elements.Add(emptyBucketListElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("../Elements/Index");
			}

			return Page();
		}
    }
}
