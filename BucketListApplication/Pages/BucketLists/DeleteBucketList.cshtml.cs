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

namespace BucketListApplication.Pages.BucketLists
{
    public class DeleteBucketListModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public DeleteBucketListModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public string ErrorMessage { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            BucketList = await _context.BucketLists
                         .Include(bl => bl.BLElements)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(bl => bl.BucketListID == id);

            if (BucketList == null)
                return NotFound();

            //Not the owner tries to delete their BucketList
            if (BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            if (saveChangesError.GetValueOrDefault())
                ErrorMessage = "Delete failed. Try again";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var BucketListToRemove = await _context.BucketLists.FindAsync(id);

            if (BucketListToRemove == null)
                return NotFound();

            //Not the owner tries to delete their BucketList
            if (BucketListToRemove.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            try
            {
                _context.BucketLists.Remove(BucketListToRemove);
                await _context.SaveChangesAsync();
                return RedirectToPage("../BLElements/Index");
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