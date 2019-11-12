using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class Design
    {
        public int DesignID { get; set; }
        public string PictureURL { get; set; }
        public Color BorderColor { get; set; }
        public Color BackgroundColor { get; set; }

        public BorderType BorderType { get; set; }
    }
}
