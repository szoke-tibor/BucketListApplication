using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class BLUser : IdentityUser
    {
        public string ProfileImageURL { get; set; }
        public string About { get; set; }
        public string FullName { get; set; }

        public ICollection<BucketList> Lists { get; set; }
    }
}
