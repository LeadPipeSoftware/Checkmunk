using System.Collections.Generic;

namespace Checkmunk.Contracts.Checklists.V1.Models
{
    public class UpdateChecklistModel
    {
		public virtual string Title { get; set; }

		public virtual IList<UpdateChecklistItemModel> Items { get; set; }
	}
}
