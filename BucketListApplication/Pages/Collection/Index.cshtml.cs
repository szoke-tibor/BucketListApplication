using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Models.BLViewModels;
using System.Collections.Generic;
using BucketListApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BucketListApplication.Pages.Collection
{
    public class IndexModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IBucketListService _bucketListService;

        public IndexModel(BLContext context, IBucketListService bucketListService)
        {
            _context = context;
            _bucketListService = bucketListService;
        }

        public IEnumerable<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }
        public IEnumerable<Element> SelectedCategoryElements { get; set; }

        public async Task<IActionResult> OnGetAsync(int? categoryId)
        {
            Categories = await _bucketListService.GetCategoriesOrderedByNameWithElementsAsync(_context);

            if (categoryId != null)
            {
                SelectedCategory = _bucketListService.GetCategoryByID(Categories, categoryId);
                if (SelectedCategory == null)
                    return NotFound();
                SelectedCategoryElements = _bucketListService.GetElementsOfCategoryOrderedByName(SelectedCategory);
            }

            return Page();
        }
    }
}
