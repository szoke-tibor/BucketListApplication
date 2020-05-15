using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models.BLViewModels
{
	public class AssignedCategoryData
	{
		public int CategoryID { get; set; }
		public string Name { get; set; }
		public bool Assigned { get; set; }
	}
}
