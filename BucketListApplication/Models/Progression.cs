using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
	public class Progression
	{
		public int ProgressionID { get; set; }
		public int ElementID { get; set; }
		public IList<BLETask> BLETasks { get; set; }
		public BucketListElement BLElement { get; set; }

		public void DeleteEmptyTasks()
		{
			for( int i = BLETasks.Count - 1; i > 0 ; i--)
				if (BLETasks[i].Text == null)
					BLETasks.Remove(BLETasks[i]);
		}

		[NotMapped]
		public double CompletedPercentage
		{
			get
			{
				int CompletedCounter = 0;
				foreach (BLETask task in BLETasks)
					if (task.Completed)
						CompletedCounter++;
				return CompletedCounter * 100 / BLETasks.Count();
			}
		}
	}
}
