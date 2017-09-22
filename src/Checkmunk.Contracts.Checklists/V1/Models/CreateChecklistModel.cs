using System.Collections.Generic;

namespace Checkmunk.Contracts.Checklists.V1.Models
{
    public class CreateChecklistModel
    {
        public virtual UserModel CreatedBy { get; set; }

		public virtual string Title { get; set; }

		public virtual IList<CreateChecklistItemModel> Items { get; set; }
	}
}
