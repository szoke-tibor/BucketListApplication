using BucketListApplication.Data;
using BucketListApplication.Models;
using BucketListApplication.Models.BLViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Interfaces
{
	public interface IBucketListService
	{
        public void PopulateAssignedCategoryData(BLContext context, BucketListElement BLElement, ref List<AssignedCategoryData> assignedCategoryDataList);
        public void PopulateBucketListDropDownList(BLContext context, string userId, ref SelectList BucketListSL, object selectedBucketList = null);
        public Task UpdateBLElementCategories(BLContext context, string[] selectedCategories, BucketListElement BLElementToUpdate);
    }
}
