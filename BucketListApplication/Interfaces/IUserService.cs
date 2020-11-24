using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BucketListApplication.Data;
using BucketListApplication.Models;

namespace BucketListApplication.Interfaces
{
	public interface IUserService
	{
		public bool BucketListIsNotBelongingToUser(ClaimsPrincipal user, BucketList bl);
		public bool BucketListElementIsNotBelongingToUser(ClaimsPrincipal user, BucketListElement ble);
		public bool UserIsNotAuthenticated(ClaimsPrincipal user);
		public string GetUserId(ClaimsPrincipal user);
		public Task<IEnumerable<BLUser>> SearchUsers(BLContext context, string searchString, ClaimsPrincipal User);
		public Task<BLUser> FindUserByID(BLContext context, string userId);
	}
}
