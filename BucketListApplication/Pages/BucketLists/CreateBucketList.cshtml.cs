using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;

namespace BucketListApplication.Pages.BucketLists
{
    public class CreateBucketListModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public CreateBucketListModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BucketList BucketList { get; set; }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("../AuthError");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("../AuthError");

            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            BucketList.UserId = CurrentUserId;

            var emptyBucketList = new BucketList();

            if (await TryUpdateModelAsync<BucketList>(
                emptyBucketList,
                "BucketList",
                bl => bl.Name, bl => bl.UserId))
            {
                _context.BucketLists.Add(BucketList);
                await _context.SaveChangesAsync();
                return RedirectToPage("../BLElements/Index");
            }
            return Page();
        }
    }
}
