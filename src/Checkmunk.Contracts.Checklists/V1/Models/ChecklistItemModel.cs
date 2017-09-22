using System;

namespace Checkmunk.Contracts.Checklists.V1.Models
{
    public class ChecklistItemModel
    {
        //public virtual int ChecklistId { get; set; }

		public virtual Guid Id { get; set; }

		public virtual bool IsChecked { get; set; }

		public virtual string Text { get; set; }
	}
}
