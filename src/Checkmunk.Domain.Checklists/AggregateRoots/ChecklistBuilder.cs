using System;
using System.Collections.Generic;
using Checkmunk.Domain.Checklists.Entities;
using Checkmunk.Domain.Checklists.ValueObjects;

namespace Checkmunk.Domain.Checklists.AggregateRoots
{
    public class ChecklistBuilder : IChecklistTitle,
                                    IChecklistCreatedByUser,
                                    IChecklistOptionalValues
    {
        private string title;
        private User createdBy;
        private DateTime createdAt = DateTime.UtcNow;
        private Guid id = Guid.NewGuid();
        private IList<ChecklistItem> items = new List<ChecklistItem>();

        private ChecklistBuilder()
        {
        }

        public static implicit operator Checklist(ChecklistBuilder builder)
        {
            return builder.Finish();
        }

        public static IChecklistTitle Build()
        {
            return new ChecklistBuilder();
        }

        public IChecklistCreatedByUser WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public IChecklistOptionalValues ByUser(User createdBy)
        {
            this.createdBy = createdBy;
            return this;
        }

        public IChecklistOptionalValues OnDate(DateTime onDate)
        {
            this.createdAt = onDate;
            return this;
        }

        public IChecklistOptionalValues WithItems(IList<ChecklistItem> items)
        {
            this.items = items;
            return this;
        }

        public Checklist Finish()
        {
            var checklist = new Checklist(id, title, createdBy, createdAt, items);

            return checklist;
        }
    }
}
