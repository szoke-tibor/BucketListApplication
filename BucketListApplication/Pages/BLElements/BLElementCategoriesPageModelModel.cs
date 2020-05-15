using BucketListApplication.Data;
using BucketListApplication.Models;
using BucketListApplication.Models.BLViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace BucketListApplication.Pages.BLElements
{
    public class BLElementCategoriesPageModel : PageModel
    {

        public List<AssignedCategoryData> AssignedCategoryDataList;

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

            //Categories selected at editing
            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            //Categories before editing
            var BLElementCategories = new HashSet<int>
                (BLElementToUpdate.ElementCategories.Select(ec => ec.Category.CategoryID));
            foreach (var category in context.Categories)
            {
                if (selectedCategoriesHS.Contains(category.CategoryID.ToString()))
                {
                    //Selected categories contain, but old categories don't -> Add
                    if (!BLElementCategories.Contains(category.CategoryID))
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
                    if (BLElementCategories.Contains(category.CategoryID))
                    {
                        ElementCategory categoryToRemove =
                            BLElementToUpdate.ElementCategories.SingleOrDefault(ec => ec.CategoryID == category.CategoryID);
                        context.Remove(categoryToRemove);
                    }
                }
            }
        }
    }
}