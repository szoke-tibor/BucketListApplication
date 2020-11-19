using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BucketListApplication.Models
{
    public class BLUser : IdentityUser
    {
		[PersonalData]
		public string ProfileImageURL { get; set; }
		[PersonalData]
		public string About { get; set; }
		[PersonalData]
		public string FullName { get; set; }
		public bool SeededUser { get; set; }
        public ICollection<BucketList> Lists { get; set; }
    }
}
