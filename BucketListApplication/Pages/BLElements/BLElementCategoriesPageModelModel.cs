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

namespace BucketListApplication.Pages.BLElements
{
    public class BLElementCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList;

        public SelectList DesignSL { get; set; }
        public SelectList BucketListSL { get; set; }

        public void PopulateAssignedCategoryData(BLContext context, BucketListElement BLElement)
        {
            var allCategories = context.Categories;
            var BLCategories = new HashSet<int>(
                BLElement.ElementCategories.Select(ec => ec.CategoryID));
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var category in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Assigned = BLCategories.Contains(category.CategoryID)
                });
            }
        }

        public void UpdateBLElementCategories(BLContext context, string[] selectedCategories, BucketListElement BLElementToUpdate)
        {
            if (selectedCategories == null)
            {
                BLElementToUpdate.ElementCategories = new List<ElementCategory>();
                return;
            }

            //Categories before editing
            var BLElementCategoriesBeforeEdit = new HashSet<int>
                (BLElementToUpdate.ElementCategories.Select(ec => ec.Category.CategoryID));
            //Categories selected at editing
            var BLElementCategoriesAfterEdit = new HashSet<string>(selectedCategories);
            
            foreach (var category in context.Categories)
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
                        context.Remove(categoryToRemove);
                    }
                }
            }
        }

        public void PopulateDesignDropDownList(BLContext _context, object selectedDesign = null)
        {
            var designsQuery = from d in _context.Designs
                               orderby d.Name
                               select d;

            DesignSL = new SelectList(designsQuery.AsNoTracking(), nameof(Models.Design.DesignID), nameof(Models.Design.Name), selectedDesign);
        }

        public void PopulateBucketListDropDownList(BLContext _context, object selectedBucketList = null)
        {
            //Logged user's userId
            var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
            {
                var usersBucketListsQuery = from bl in _context.BucketLists
                                            where bl.UserId == CurrentUserId
                                            orderby bl.Name
                                            select bl;

                BucketListSL = new SelectList(usersBucketListsQuery.AsNoTracking(), nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name), selectedBucketList);
            }
            else
                throw new Exception("Nincs bejelentkezett felhasználó.");
        }
    }
}