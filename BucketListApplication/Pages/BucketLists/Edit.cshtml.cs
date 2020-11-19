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
using System.Security.Claims;

namespace BucketListApplication.Pages.BucketLists
{
    public class EditModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public EditModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Logged user's userId
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
            {
                if (id == null)
                    return NotFound();

                BucketList = await _context.BucketLists
                    .FirstOrDefaultAsync(m => m.BucketListID == id);

                if (BucketList == null)
                    return NotFound();

                //Not the owner tries to edit their BucketListElement
                if (BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                    return Forbid();

                return Page();
            }
            else
                return RedirectToPage("../AuthError");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var bucketlistToUpdate = await _context.BucketLists
                .FirstOrDefaultAsync(bl => bl.BucketListID == id);

            if (bucketlistToUpdate == null)
                return NotFound();

            //Not the owner tries to edit their BucketListElement
            if (bucketlistToUpdate.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            // Defense against overposting attacks. Returns true if the update was successful.
            if (await TryUpdateModelAsync<BucketList>(bucketlistToUpdate, "BucketList",
                bl => bl.Name,
                bl => bl.Visibility))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("../BucketLists/Index", new { selectedbucketlistid = bucketlistToUpdate.BucketListID });
            }
            return Page();
        }
    }
}
