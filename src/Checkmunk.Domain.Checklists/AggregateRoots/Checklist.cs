using System;
using System.Collections.Generic;
using System.Linq;
using Checkmunk.Domain.Common;
using Checkmunk.Domain.Checklists.Entities;
using Checkmunk.Domain.Checklists.ValueObjects;
using LeadPipe.Net;

namespace Checkmunk.Domain.Checklists.AggregateRoots
{
    public class Checklist : IAmPersistable, IAmAuditable<User>
    {
		private DateTime createdAt;
		private User createdBy;
		private Guid id;
		private IList<ChecklistItem> items;
        private string title;

        internal Checklist(Guid id, string title, User createdBy, DateTime createdAt, IList<ChecklistItem> items)
        {
            // For building new objects (called by the ChecklistFactory)

            Guard.Will.ProtectAgainstDefaultValueArgument(() => id);
            Guard.Will.ProtectAgainstNullOrEmptyStringArgument(() => title);
            Guard.Will.ProtectAgainstNullArgument(() => createdBy);
            Guard.Will.ProtectAgainstDefaultValueArgument(() => createdAt);
            Guard.Will.ProtectAgainstNullArgument(() => items);

            this.createdAt = createdAt;
            this.createdBy = createdBy;
            this.id = id;
            this.title = title;

			this.items = items;

            foreach (var item in this.items)
            {
                item.MoveToChecklist(this);
            }
		}

        protected internal Checklist()
        {
            // For O/RM use
        }

		public virtual DateTime CreatedAt
		{
			get => createdAt;
			protected set => createdAt = value;
		}

		public virtual User CreatedBy
		{
            get => createdBy;
		    protected set => createdBy = value;
		}

		public virtual Guid Id
        {
            get => id;
            protected set => id = value;
        }

        public virtual string Title
        {
            get => title;
            protected set => title = value;
        }

        public virtual IList<ChecklistItem> Items
        {
            get => items;
            protected set => items = value;
        }

        public virtual int PersistenceId { get; set; }

        public virtual void AddCheckbox(string text)
        {
            Guard.Will.ProtectAgainstNullOrEmptyStringArgument(() => text);

            items.Add(new ChecklistItem(this, text));
        }

        public virtual void ChangeTitle(string title)
        {
            Guard.Will.ProtectAgainstNullOrEmptyStringArgument(() => title);

            this.title = title;
        }

        public virtual IEnumerable<ChecklistItem> ReorderItems(IEnumerable<int> order)
        {
            /*
             * The order list should tell us what position each of the list
             * items should be in after we re-order. For example:
             * 
             * Given {"C", "A", "B"}
             * When var result = ReorderItems(new List<int>(1, 2, 0))
             * Then result == {"A", "B", "C"}
             */

            items = items = order.Select(i => items[i]).ToList();

            return items;
        }
    }
}
