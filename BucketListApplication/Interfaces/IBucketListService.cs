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
        public Task<List<AssignedCategoryData>> PopulateAssignedCategoryData(BLContext context, BucketListElement BLElement);
        public Task<SelectList> PopulateBucketListDropDownList(BLContext context, string userId, bool publicOnly, bool addDefaultValue, object selectedBucketList = null);
        public Task UpdateBLElementCategories(BLContext context, string[] selectedCategories, BucketListElement BLElementToUpdate);
        public Task<IEnumerable<BucketListElement>> PopulateSelectedBLElementsList(BLContext context, int SelectedBucketListID, bool PublicOnly);

        public Task<BucketListElement> InitializeBLE(BLContext context, int? bucketListId);
        public void AddCategoriesToBLE(string[] selectedCategories, BucketListElement newBLElement);
        public Task<BucketListElement> GetBLEByID_WithBLAsync(BLContext context, int? bucketListElementId);
        public Task<BucketListElement> GetBLEByID_WithBLETasksAndCategoryAsync(BLContext context, int? bucketListElementId);
        public void DeleteEmptyTasks(IList<BLETask> BLETasks);
        public Task<BucketList> GetBLByID_WithBLEsAsync(BLContext context, int? bucketListId);
        public Task<BucketList> GetBLByID_Async(BLContext context, int? bucketListId);
        public Task<IEnumerable<Category>> GetCategories_WithElementsAsync(BLContext context);
        public Category GetCategoryByID(IEnumerable<Category> Categories, int? categoryId);
        public IEnumerable<Element> GetElementsOfCategory(Category SelectedCategory);
        public string SetUserCheckPageTitle(BLUser SelectedUser);
    }
}
