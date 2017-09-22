using System;
using System.Collections.Generic;

namespace Checkmunk.Contracts.Checklists.V1.Models
{
    public class ChecklistModel
    {
        public virtual DateTime CreatedAt { get; set; }

        public virtual UserModel CreatedBy { get; set; }

		public virtual Guid Id { get; set; }

		public virtual string Title { get; set; }

		public virtual IList<ChecklistItemModel> Items { get; set; }
	}
}
