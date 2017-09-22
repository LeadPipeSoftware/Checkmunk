namespace Checkmunk.Contracts.Checklists.V1.Models
{
    public class CreateChecklistItemModel
    {
		public virtual bool IsChecked { get; set; }

		public virtual string Text { get; set; }
	}
}
