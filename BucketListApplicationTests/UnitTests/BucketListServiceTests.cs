using BucketListApplication.Data;
using BucketListApplication.Models;
using BucketListApplication.Services;
using System;
using System.Collections.Generic;
using System.Text;
using BucketListApplicationTests.Utility;
using Xunit;
using System.Linq;
using BucketListApplication.Interfaces;
using System.Threading.Tasks;

namespace BucketListApplicationTests.UnitTests
{
	public class BucketListServiceTests
    {
        private readonly IBucketListService _bucketListService;
        public BucketListServiceTests()
		{
            _bucketListService = new BucketListService();
        }

		[Fact]
		public async Task PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync_PopulateBLEs_WhenPublicBLEsAreRequired()
		{
			using (var db = new BLContext(Utilities.TestDbContextOptions()))
			{
				// Arrange
				BucketListElement publicBLE2 = new BucketListElement()
				{
					Name = "2",
					Visibility = Visibility.Public,
					BucketListID = 1,
					Progression = new Progression() { BLETasks = new List<BLETask>() }
				};
				BucketListElement publicBLE1 = new BucketListElement()
				{
					Name = "1",
					Visibility = Visibility.Public,
					BucketListID = 1,
					Progression = new Progression() { BLETasks = new List<BLETask>() }
				};
				BucketListElement privateBLE1 = new BucketListElement()
				{
					Name = "3",
					Visibility = Visibility.Private,
					BucketListID = 1,
					Progression = new Progression() { BLETasks = new List<BLETask>() }
				};
				await db.Elements.AddAsync(publicBLE2);
				await db.Elements.AddAsync(publicBLE1);
				await db.Elements.AddAsync(privateBLE1);
				await db.SaveChangesAsync();

				// Act
				var actualBucketListElements = await _bucketListService.PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync(db, 1, true);

				// Assert
				Assert.Equal(2, actualBucketListElements.Count());
				Assert.Equal(publicBLE1, actualBucketListElements.ElementAt(0));
				Assert.Equal(publicBLE2, actualBucketListElements.ElementAt(1));
				Assert.NotNull(actualBucketListElements.ElementAt(0).Progression.BLETasks);
				Assert.NotNull(actualBucketListElements.ElementAt(1).Progression.BLETasks);
			}
		}

		[Fact]
		public async Task PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync_PopulateBLEs_WhenNotOnlyPublicBLEsAreRequired()
		{
			using (var db = new BLContext(Utilities.TestDbContextOptions()))
			{
				// Arrange
				BucketListElement publicBLE2 = new BucketListElement()
				{
					Name = "2",
					Visibility = Visibility.Public,
					BucketListID = 1,
					Progression = new Progression() { BLETasks = new List<BLETask>() }
				};
				BucketListElement publicBLE1 = new BucketListElement()
				{
					Name = "1",
					Visibility = Visibility.Public,
					BucketListID = 1,
					Progression = new Progression() { BLETasks = new List<BLETask>() }
				};
				BucketListElement privateBLE1 = new BucketListElement()
				{
					Name = "3",
					Visibility = Visibility.Private,
					BucketListID = 1,
					Progression = new Progression() { BLETasks = new List<BLETask>() }
				};
				await db.Elements.AddAsync(publicBLE2);
				await db.Elements.AddAsync(publicBLE1);
				await db.Elements.AddAsync(privateBLE1);
				await db.SaveChangesAsync();

				// Act
				var actualBucketListElements = await _bucketListService.PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync(db, 1, false);

				// Assert
				Assert.Equal(3, actualBucketListElements.Count());
				Assert.Equal(publicBLE1, actualBucketListElements.ElementAt(0));
				Assert.Equal(publicBLE2, actualBucketListElements.ElementAt(1));
				Assert.Equal(privateBLE1, actualBucketListElements.ElementAt(2));
				Assert.NotNull(actualBucketListElements.ElementAt(0).Progression.BLETasks);
				Assert.NotNull(actualBucketListElements.ElementAt(1).Progression.BLETasks);
				Assert.NotNull(actualBucketListElements.ElementAt(2).Progression.BLETasks);
			}
		}

		[Fact]
		public void DeleteEmptyTasks_EmptyTaskIsDeleted_WhenEmptyTaskIsFound()
		{
			// Arrange
			List<BLETask> BLETasks = new List<BLETask>()
			{
				new BLETask { BLETaskID = 1, Text = null },
				new BLETask { BLETaskID = 2, Text = "text2" },
				new BLETask { BLETaskID = 3, Text = "text3" }
			};
			var expectedBLETasks = BLETasks.Where(task => task.BLETaskID != 1).ToList();

			// Act
			_bucketListService.DeleteEmptyTasks(BLETasks);

			// Assert
			Assert.Equal(expectedBLETasks, BLETasks);
		}

		[Fact]
		public async Task GetBLByIDAsync_BucketListIsReturned_WhenItIsFoundInDatabase()
		{
			using (var db = new BLContext(Utilities.TestDbContextOptions()))
			{
				// Arrange
				BucketList expectedBucketList = new BucketList() { BucketListID = 1 };
				await db.BucketLists.AddAsync(expectedBucketList);
				await db.SaveChangesAsync();

				// Act
				var actualBucketList = await _bucketListService.GetBLByIDAsync(db, 1);

				// Assert
				Assert.Equal(expectedBucketList, actualBucketList);
			}
		}

		[Fact]
		public async Task GetCategoriesOrderedByNameWithElementsAsync_CategoriesReturnedInNameOrder_WhenDatabaseHasCategories()
		{
			using (var db = new BLContext(Utilities.TestDbContextOptions()))
			{
				// Arrange
				var expectedCategories = new Category[]
				{
					new Category
					{
						Name = "1",
						ElementCategories = new HashSet<ElementCategory>()
						{
							new ElementCategory
							{
								Element = new BucketListElement { Name = "ble" }
							}
						}
					},
					new Category
					{
						Name = "0",
						ElementCategories = new HashSet<ElementCategory>()
						{
							new ElementCategory
							{
								Element = new Element { Name = "e" }
							}
						}
					}
				};
				await db.Categories.AddRangeAsync(expectedCategories);
				await db.SaveChangesAsync();

				// Act
				var actualCategories = await _bucketListService.GetCategoriesOrderedByNameWithElementsAsync(db);

				// Assert
				Assert.Equal(2, actualCategories.Count());
				Assert.Equal(expectedCategories[1], actualCategories.ElementAt(0));
				Assert.Equal(expectedCategories[0], actualCategories.ElementAt(1));
				Assert.NotNull(actualCategories.ElementAt(0).ElementCategories.ElementAt(0).Element);
				Assert.NotNull(actualCategories.ElementAt(1).ElementCategories.ElementAt(0).Element);
			}
		}

		[Fact]
		public void GetCategoryByID_CategoryIsReturned_WhenCategoryIsFound()
		{
			// Arrange
			Category expectedCategory = new Category { CategoryID = 1 };
			Category otherCategory = new Category { CategoryID = 2 };
			IEnumerable<Category> categoryList = new List<Category>
			{
				expectedCategory,
				otherCategory
			};

			// Act
			Category actualCategory = _bucketListService.GetCategoryByID(categoryList, 1);

			// Assert
			Assert.Equal(expectedCategory, actualCategory);
		}

		[Fact]
		public void GetCategoryByID_NULLIsReturned_WhenCategoryIsNotFound()
		{
			// Arrange
			Category exampleCategory = new Category { CategoryID = 1 };
			Category otherCategory = new Category { CategoryID = 2 };
			IEnumerable<Category> categoryList = new List<Category>
			{
				exampleCategory,
				otherCategory
			};

			// Act
			Category foundCategory = _bucketListService.GetCategoryByID(categoryList, 3);

			// Assert
			Assert.Null(foundCategory);
		}

		[Fact]
		public void GetElementsOfCategoryOrderedByName_OnlyElementsAreReturnedInNameOrder_WhenThereAreBucketListElementsInTheListAsWell()
		{
			// Arrange
			Category category = new Category
			{
				ElementCategories = new List<ElementCategory>()
				{
					new ElementCategory
					{
						Element = new Element { Name = "1", Discriminator = "Element"}
					},
					new ElementCategory
					{
						Element = new Element { Name = "0", Discriminator = "Element"}
					},
					new ElementCategory
					{
						Element = new BucketListElement { Name = "2", Discriminator = "BucketListElement" }
					}
				}
			};

			// Act
			var elements = _bucketListService.GetElementsOfCategoryOrderedByName(category);

			// Assert
			Assert.Equal(2, elements.Count());
			Assert.Equal(category.ElementCategories.ElementAt(1).Element, elements.ElementAt(0));
			Assert.Equal(category.ElementCategories.ElementAt(0).Element, elements.ElementAt(1));
		}

		[Fact]
		public void SetUserCheckPageTitle_UserCheckPageTitleIsSet_WhenUserFullNameIsNotNull()
		{
			// Arrange
			BLUser User = new BLUser { FullName = "Teszt Elek" };
			string expectedTitle = "Teszt Elek Bakancslistái";

			// Act
			string actualTitle = _bucketListService.SetUserCheckPageTitle(User);

			// Assert
			Assert.Equal(expectedTitle, actualTitle);
		}
	}
}
