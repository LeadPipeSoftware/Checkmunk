using System;
using Checkmunk.Domain.Common;
using Checkmunk.Domain.Checklists.AggregateRoots;
using LeadPipe.Net;

namespace Checkmunk.Domain.Checklists.ValueObjects
{
    public class ChecklistItem : IAmPersistable
    {
        private Checklist checklist;
        private bool isChecked;
        private Guid id;
        private string text;

        public ChecklistItem(Checklist checklist, string text)
        {
            id = Guid.NewGuid();
            this.checklist = checklist;
            this.text = text;
        }

        public ChecklistItem(string text)
        {
            // WARNING: Using this constructor will result in an orphaned checkbox!

            id = Guid.NewGuid();
            this.text = text;
        }

        protected internal ChecklistItem()
        {
            // Required by the ORM
        }

        public virtual Checklist Checklist
        {
            get => checklist;
        }

		public virtual Guid Id
        {
            get => id;
            protected set => id = value;
        }

		public virtual bool IsChecked
        {
            get => isChecked;
            protected set => isChecked = value;
        }

        public virtual int PersistenceId { get; set; }

		public virtual string Text
        {
            get => text;
            protected set => text = value;
        }

        public virtual void MoveToChecklist(Checklist checklist)
        {
            Guard.Will.ProtectAgainstNullArgument(() => checklist);

            this.checklist = checklist;
        }
    }
}
