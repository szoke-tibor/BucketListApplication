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
        public Task<List<AssignedCategoryData>> PopulateAssignedCategoryDataAsync(BLContext context, BucketListElement BLElement);
        public Task<SelectList> PopulateBucketListDropDownListOrderedByNameAsync(BLContext context, string userId, bool publicOnly, bool addDefaultValue, object selectedBucketList = null);
        public Task UpdateBLElementCategoriesAsync(BLContext context, string[] selectedCategories, BucketListElement BLElementToUpdate);
        public Task<IEnumerable<BucketListElement>> PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync(BLContext context, int SelectedBucketListID, bool PublicOnly);

        public Task<BucketListElement> InitializeBLEWithBLAsync(BLContext context, int? bucketListId);
        public void AddCategoriesToBLE(string[] selectedCategories, BucketListElement newBLElement);
        public Task<BucketListElement> GetBLEByIDWithBLAsync(BLContext context, int? bucketListElementId);
        public Task<BucketListElement> GetBLEByIDWithBLETasksAndCategoryAsync(BLContext context, int? bucketListElementId);
        public void DeleteEmptyTasks(IList<BLETask> BLETasks);
        public Task<BucketList> GetBLByIDWithBLEsAsync(BLContext context, int? bucketListId);
        public Task<BucketList> GetBLByIDAsync(BLContext context, int? bucketListId);
        public Task<IEnumerable<Category>> GetCategoriesOrderedByNameWithElementsAsync(BLContext context);
        public Category GetCategoryByID(IEnumerable<Category> Categories, int? categoryId);
        public IEnumerable<Element> GetElementsOfCategoryOrderedByName(Category SelectedCategory);
        public string SetUserCheckPageTitle(BLUser SelectedUser);
    }
}
