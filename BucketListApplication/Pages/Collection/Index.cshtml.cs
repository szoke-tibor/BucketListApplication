using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Models.BLViewModels;
using System.Collections.Generic;
using BucketListApplication.Interfaces;

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

        public async Task OnGetAsync(int? categoryId)
        {
            Categories = await _bucketListService.GetCategories_WithElementsAsync(_context);

            if (categoryId != null)
            {
                SelectedCategory = _bucketListService.GetCategoryByID(Categories, categoryId);
                SelectedCategoryElements = _bucketListService.GetElementsOfCategory(SelectedCategory);
            }
        }
    }
}
