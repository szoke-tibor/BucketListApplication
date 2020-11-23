using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BucketListApplication.Interfaces;
using BucketListApplication.Models;

namespace BucketListApplication.Services
{
	public class UserService : IUserService
	{
		public bool BucketListElementIsNotBelongingToUser(ClaimsPrincipal user, BucketListElement ble)
		{
			return (ble.BucketList.UserId != user.FindFirstValue(ClaimTypes.NameIdentifier));
		}

		public bool BucketListIsNotBelongingToUser(ClaimsPrincipal user, BucketList bl)
		{
			return (bl.UserId != user.FindFirstValue(ClaimTypes.NameIdentifier));
		}
		public bool UserIsNotAuthenticated(ClaimsPrincipal user)
		{
			return !user.Identity.IsAuthenticated;
		}
		public string GetUserId(ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
