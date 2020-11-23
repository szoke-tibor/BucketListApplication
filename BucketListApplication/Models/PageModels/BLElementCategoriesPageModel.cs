using BucketListApplication.Data;
using BucketListApplication.Models;
using BucketListApplication.Models.BLViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BucketListApplication.Pages.BLElements
{
    public class BLElementCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList;
        public SelectList BucketListSL;
    }
}