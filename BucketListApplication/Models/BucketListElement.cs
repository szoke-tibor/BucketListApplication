﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class BucketListElement : Element
    {
		public int ListID { get; set; }
		public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
