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
        public void PopulateBucketListDropDownList(BLContext context, string userId, ref SelectList BucketListSL, bool PublicOnly, bool addDefaultValue, object selectedBucketList = null);
        public Task UpdateBLElementCategories(BLContext context, string[] selectedCategories, BucketListElement BLElementToUpdate);
        public void PopulateSelectedBLElementsList(BLContext context, int SelectedBucketListID, bool PublicOnly, ref IEnumerable<BucketListElement> SelectedBLElements);

        public Task<BucketListElement> Initialize(BLContext context, int? bucketListId);
        public void AddCategoriesToBLE(string[] selectedCategories, BucketListElement newBLElement);
        public Task<BucketListElement> GetBLEByID(BLContext context, int? bucketListElementId);
        public Task<BucketListElement> GetBLEByIDWithDetails(BLContext context, int? bucketListElementId);
        public void DeleteEmptyTasks(IList<BLETask> BLETasks);
        public Task<BucketList> GetBLByIDWithBLEs(BLContext context, int? bucketListId);
        public Task<BucketList> GetBLByIDToDelete(BLContext context, int? bucketListId);
    }
}
