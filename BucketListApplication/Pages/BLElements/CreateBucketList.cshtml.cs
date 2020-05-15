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

namespace BucketListApplication.Pages.BLElements
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
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyBucketList = new BucketList();

            //Logged user's userId
            var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
                BucketList.UserId = CurrentUserId;
            else
                return RedirectToPage("../Index");

            if (await TryUpdateModelAsync<BucketList>(
                emptyBucketList,
                "BucketList",
                bl => bl.Name, bl => bl.UserId))
            {
                _context.BucketLists.Add(BucketList);
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
