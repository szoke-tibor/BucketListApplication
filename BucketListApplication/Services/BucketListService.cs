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
        public void PopulateAssignedCategoryData(BLContext context, BucketListElement BLElement, ref List<AssignedCategoryData> assignedCategoryDataList)
        {
            var allCategories = context.Categories
                .AsNoTracking()
                .ToList();
            var BLCategories = new HashSet<int>(BLElement.ElementCategories.Select(ec => ec.CategoryID));
            assignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var category in allCategories)
            {
                assignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Assigned = BLCategories.Contains(category.CategoryID)
                });
            }
        }

        public void PopulateBucketListDropDownList(BLContext context, string userId, ref SelectList BucketListSL, object selectedBucketList = null)
        {
            var usersBucketListsQuery = context.BucketLists
                .AsNoTracking()
                .Where(bl => bl.UserId == userId)
                .OrderBy(bl => bl.Name)
                .ToList();

            BucketListSL = new SelectList(usersBucketListsQuery, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name), selectedBucketList);
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
    }
}
