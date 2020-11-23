using BucketListApplication.Data;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Pages.BLElements
{
	public class BLElementListingPageModel : PageModel
	{
		public SelectList BucketListSL;
		public IEnumerable<BucketListElement> SelectedBLElements;
	}
}
