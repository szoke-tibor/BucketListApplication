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
		/*
		public Int32 BorderColorARGB
		{
			get { return BorderColor.ToArgb(); }
			set
			{
				BorderColorARGB = value;
				BorderColor = Color.FromArgb(value);
			}
		}

		public Int32 BackgroundColorARGB
		{
			get { return BackgroundColor.ToArgb(); }
			set
			{
				BackgroundColorARGB = value;
				BackgroundColor = Color.FromArgb(value);
			}
		}
		
		[NotMapped]
        public Color BorderColor
		{
			get { return BorderColor; }
			set 
			{ 
				BorderColor = value;
				BorderColorARGB = value.ToArgb();
			}
		}
			
		[NotMapped]
		public Color BackgroundColor
		{
			get { return BackgroundColor; }
			set
			{
				BackgroundColor = value;
				BackgroundColorARGB = value.ToArgb();
			}
		}
		*/
		public BorderType BorderType { get; set; }
    }

	public enum BorderType
	{
		Regular,
		Fancy
	}
}
