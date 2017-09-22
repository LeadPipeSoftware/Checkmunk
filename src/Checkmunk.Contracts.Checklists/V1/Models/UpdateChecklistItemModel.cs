namespace Checkmunk.Contracts.Checklists.V1.Models
{
    public class UpdateChecklistItemModel
    {
		public virtual bool IsChecked { get; set; }

		public virtual string Text { get; set; }
	}
}
