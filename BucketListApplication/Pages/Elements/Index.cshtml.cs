﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;

namespace BucketListApplication.Pages.Elements
{
    public class IndexModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public IndexModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public IList<Element> Element { get;set; }

        public async Task OnGetAsync()
        {
            Element = await _context.Elements.ToListAsync();
        }
    }
}