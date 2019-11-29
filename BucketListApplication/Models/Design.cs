using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class Design
    {
        public int DesignID { get; set; }
		public string Name { get; set; }
        public string PictureURL { get; set; }
		public Int32 BorderColorARGB { get; set; }
		public Int32 BackgroundColorARGB { get; set; }
		public BorderType BorderType { get; set; }

		[NotMapped]
        public Color BorderColor
		{
			get { return Color.FromArgb(BorderColorARGB); }
			set { BorderColorARGB = value.ToArgb(); }
		}
			
		[NotMapped]
		public Color BackgroundColor
		{
			get { return Color.FromArgb(BackgroundColorARGB); }
			set { BackgroundColorARGB = value.ToArgb(); }
		}
    }

	public enum BorderType
	{
		Regular,
		Fancy
	}
}
