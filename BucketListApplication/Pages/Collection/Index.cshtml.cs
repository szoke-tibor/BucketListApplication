using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;
using BucketListApplication.Models.BLViewModels;

namespace BucketListApplication.Pages.Collection
{
    public class IndexModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public IndexModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public CategoryIndexData CategoryData { get; set; }
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id)
        {
            CategoryData = new CategoryIndexData();
            CategoryData.Categories = await _context.Categories
                .Include(c => c.ElementCategories)
                    .ThenInclude(ec => ec.Element)
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            if (id != null)
            {
                CategoryID = id.Value;
                Category category = CategoryData.Categories.Where(c => c.CategoryID == id.Value).Single();
                CategoryData.Elements = category.ElementCategories
                    .Select(ec => ec.Element)
                    .OrderBy(e => e.Name);
            }
        }
    }
}
