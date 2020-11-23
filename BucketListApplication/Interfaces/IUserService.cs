using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BucketListApplication.Models;

namespace BucketListApplication.Interfaces
{
	public interface IUserService
	{
		public bool BucketListIsNotBelongingToUser(ClaimsPrincipal user, BucketList bl);
		public bool BucketListElementIsNotBelongingToUser(ClaimsPrincipal user, BucketListElement ble);
		public bool UserIsNotAuthenticated(ClaimsPrincipal user);
		public string GetUserId(ClaimsPrincipal user);
	}
}
