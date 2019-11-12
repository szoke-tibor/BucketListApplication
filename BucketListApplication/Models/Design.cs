using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class Design
    {
        public int DesignID { get; set; }
        public string PictureURL { get; set; }
		public Int32 BorderColorARGB
		{
			get { return BorderColor.ToArgb(); }
			set { BorderColor = Color.FromArgb(value); }
		}

		public Int32 BackgroundColorARGB
		{
			get { return BackgroundColor.ToArgb(); }
			set { BackgroundColor = Color.FromArgb(value); }
		}

		[NotMapped]
        public Color BorderColor { get; set; }
		[NotMapped]
		public Color BackgroundColor { get; set; }

        public BorderType BorderType { get; set; }
    }
}
