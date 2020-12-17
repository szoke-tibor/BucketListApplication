using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BucketListApplication.Data;
using BucketListApplication.Interfaces;
using BucketListApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BucketListApplication.Services
{
	public class UserService : IUserService
	{
		public bool BucketListElementIsNotBelongingToUser(ClaimsPrincipal User, BucketListElement ble)
		{
			return (ble.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier));
		}

		public bool BucketListIsNotBelongingToUser(ClaimsPrincipal User, BucketList bl)
		{
			return (bl.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier));
		}
		public bool UserIsNotAuthenticated(ClaimsPrincipal User)
		{
			return !User.Identity.IsAuthenticated;
		}
		public string GetUserId(ClaimsPrincipal User)
		{
			return User.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		public async Task<IEnumerable<BLUser>> SearchUsers(BLContext context, string searchString, ClaimsPrincipal User)
		{
			if (!String.IsNullOrEmpty(searchString))
				return await context.Users
					.AsNoTracking()
					.Where(u => u.FullName.Contains(searchString))
					.Where(u => u.Id != GetUserId(User))
					.ToListAsync();
			else
				return await context.Users
					.AsNoTracking()
					.Where(u => u.SeededUser == true)
					.Where(u => u.Id != GetUserId(User))
					.ToListAsync();
		}

		public async Task<BLUser> FindUserByID(BLContext context, string userId)
		{
			return await context.Users.FindAsync(userId);
		}
	}
}
