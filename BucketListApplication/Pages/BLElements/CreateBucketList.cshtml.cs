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
    public class CreateBucketListModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public CreateBucketListModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BucketLists.Add(BucketList);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
