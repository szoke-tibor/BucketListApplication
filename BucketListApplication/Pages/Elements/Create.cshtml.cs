using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BucketListApplication.Data;
using BucketListApplication.Models;

namespace BucketListApplication.Pages.Elements
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
        public Element Element { get; set; }

		// DONE
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			var emptyElement = new Element();

			if (await TryUpdateModelAsync<Element>(
				emptyElement,
				"element",   // Prefix for form value.
				e => e.Name))
			{
				_context.Elements.Add(emptyElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

			return Page();
		}
    }
}
