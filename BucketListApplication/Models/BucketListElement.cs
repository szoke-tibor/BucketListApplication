using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BucketListApplication.Models
{
    public class BucketListElement : Element
    {
		public int BucketListID { get; set; }
		public string Description { get; set; }
        public bool Completed { get; set; }
		public Visibility Visibility { get; set; }

		public BucketList BucketList { get; set; }
		public Progression Progression { get; set; }
	}

	//Later Hidden can be added
	public enum Visibility
	{
		Private,
		Public
	}
}
