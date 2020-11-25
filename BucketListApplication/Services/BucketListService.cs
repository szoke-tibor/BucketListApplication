using BucketListApplication.Data;
using BucketListApplication.Interfaces;
using BucketListApplication.Models;
using BucketListApplication.Models.BLViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Services
{
	public class BucketListService : IBucketListService
    {
        public async Task<List<AssignedCategoryData>> PopulateAssignedCategoryData(BLContext context, BucketListElement BLElement)
        {
            var allCategories = await context.Categories
                .AsNoTracking()
                .ToListAsync();
            var BLCategories = new HashSet<int>(BLElement.ElementCategories.Select(ec => ec.CategoryID));
            List<AssignedCategoryData> assignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var category in allCategories)
            {
                assignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Assigned = BLCategories.Contains(category.CategoryID)
                });
            }
            return assignedCategoryDataList;
        }

        public async Task<SelectList> PopulateBucketListDropDownList(BLContext context, string userId, bool publicOnly, bool addDefaultValue, object selectedBucketList = null)
        {
            List<SelectListItem> SelectListItems = new List<SelectListItem>();
            if (addDefaultValue)
			{
                SelectListItems.Add(new SelectListItem()
                {
                    Text = "--Válassz egy listát--",
                    Value = "null"
                });
            }

            IEnumerable<BucketList> BucketListsQuery;

            if (publicOnly)
            {
                BucketListsQuery = await context.BucketLists
                    .AsNoTracking()
                    .Where(bl => bl.UserId == userId)
                    .Where(bl => bl.Visibility == Visibility.Public)
                    .OrderBy(bl => bl.Name)
                    .ToListAsync();
            }
            else
            {
                BucketListsQuery = await context.BucketLists
                    .AsNoTracking()
                    .Where(bl => bl.UserId == userId)
                    .OrderBy(bl => bl.Name)
                    .ToListAsync();
            }

            foreach (BucketList bl in BucketListsQuery)
            {
                SelectListItems.Add(new SelectListItem()
                {
                    Text = bl.Name,
                    Value = bl.BucketListID.ToString()
                });
            }

            return new SelectList(SelectListItems, "Value", "Text", selectedBucketList);
        }

        public async Task UpdateBLElementCategories(BLContext context, string[] selectedCategories, BucketListElement BLElementToUpdate)
        {
            if (selectedCategories == null)
            {
                BLElementToUpdate.ElementCategories = new List<ElementCategory>();
                return;
            }

            //Categories before editing
            var BLElementCategoriesBeforeEdit = new HashSet<int>(BLElementToUpdate.ElementCategories.Select(ec => ec.Category.CategoryID));
            //Categories selected at editing
            var BLElementCategoriesAfterEdit = new HashSet<string>(selectedCategories);
            var allCategories = await context.Categories
                .AsNoTracking()
                .ToListAsync();

            foreach (var category in allCategories)
            {
                if (BLElementCategoriesAfterEdit.Contains(category.CategoryID.ToString()))
                {
                    //Selected categories contain, but old categories don't -> Add
                    if (!BLElementCategoriesBeforeEdit.Contains(category.CategoryID))
                    {
                        ElementCategory categoryToAdd = new ElementCategory
                        {
                            ElementID = BLElementToUpdate.ElementID,
                            CategoryID = category.CategoryID
                        };
                        BLElementToUpdate.ElementCategories.Add(categoryToAdd);
                    }
                }
                else
                {
                    //Selected categories don't contain, but old categories do -> Remove
                    if (BLElementCategoriesBeforeEdit.Contains(category.CategoryID))
                    {
                        ElementCategory categoryToRemove =
                            BLElementToUpdate.ElementCategories.SingleOrDefault(ec => ec.CategoryID == category.CategoryID);
                        BLElementToUpdate.ElementCategories.Remove(categoryToRemove);
                    }
                }
            }
        }

        public async Task<IEnumerable<BucketListElement>> PopulateSelectedBLElementsList(BLContext context, int SelectedBucketListID, bool PublicOnly)
        {
            if (PublicOnly)
            {
                return await context.BLElements
                    .AsNoTracking()
                    .Include(ble => ble.Progression)
                        .ThenInclude(p => p.BLETasks)
                    .Where(ble => ble.BucketListID == SelectedBucketListID)
                    .Where(ble => ble.Visibility == Visibility.Public)
                    .OrderBy(ble => ble.Name)
                    .ToListAsync();
            }

            else
            {
                return await context.BLElements
                    .AsNoTracking()
                    .Include(ble => ble.Progression)
                        .ThenInclude(p => p.BLETasks)
                    .Where(ble => ble.BucketListID == SelectedBucketListID)
                    .OrderBy(ble => ble.Name)
                    .ToListAsync();
            }
        }

        /*CreateBLE*/
        public async Task<BucketListElement> InitializeBLE(BLContext context, int? bucketListId)
		{
            return new BucketListElement
            {
                BucketListID = bucketListId.Value,
                BucketList = await context.BucketLists.FindAsync(bucketListId),
                ElementCategories = new List<ElementCategory>(),
                Progression = new Progression()
            };
        }

        public void AddCategoriesToBLE(string[] selectedCategories, BucketListElement newBLElement)
		{
            if (selectedCategories != null)
            {
                foreach (var category in selectedCategories)
                {
                    var categoryToAdd = new ElementCategory
                    {
                        CategoryID = int.Parse(category)
                    };
                    newBLElement.ElementCategories.Add(categoryToAdd);
                }
            }
        }

        /*DeleteBLE*/
        public async Task<BucketListElement> GetBLEByID(BLContext context, int? bucketListElementId)
		{
            return await context.BLElements
                .AsNoTracking()
                .Include(ble => ble.BucketList)
                .FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);
        }

        /*DetailsBLE + EditBLE*/
        public async Task<BucketListElement> GetBLEByIDWithDetails(BLContext context, int? bucketListElementId)
        {
            return await context.BLElements
                .Include(ble => ble.BucketList)
                .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
                .FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);
        }

        /*EditBLE*/
        public void DeleteEmptyTasks(IList<BLETask> BLETasks)
        {
            for (int i = BLETasks.Count - 1; i >= 0; i--)
                if (BLETasks[i].Text == null)
                    BLETasks.Remove(BLETasks[i]);
        }

        /*DeleteBL*/
        public async Task<BucketList> GetBLByIDWithBLEs(BLContext context, int? bucketListId)
        {
            return await context.BucketLists
                .AsNoTracking()
                .Include(bl => bl.BLElements)
                .FirstOrDefaultAsync(bl => bl.BucketListID == bucketListId);
        }

        public async Task<BucketList> FindBLByID(BLContext context, int? bucketListId)
        {
            return await context.BucketLists.FindAsync(bucketListId);
        }

        /*Collection page*/
        public async Task<IEnumerable<Category>> GetCategoriesWithElements(BLContext context)
		{
            return await context.Categories
                .AsNoTracking()
                .Include(c => c.ElementCategories)
                    .ThenInclude(ec => ec.Element)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public Category GetCategoryById(IEnumerable<Category> Categories, int? categoryId)
        {
            return Categories.FirstOrDefault(c => c.CategoryID == categoryId);
        }
        public IEnumerable<Element> GetSelectedCategoryElements(Category SelectedCategory)
        {
            return SelectedCategory.ElementCategories
                    .Select(ec => ec.Element)
                    .Where(e => e.Discriminator == "Element")
                    .OrderBy(e => e.Name);
        }

        /*UserCheck*/
        public string SetUserCheckPageTitle(BLUser SelectedUser)
        {
            return SelectedUser.FullName + " Bakancslistái";
        }
    }
}
