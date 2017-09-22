using System;
using System.Collections.Generic;
using Checkmunk.Domain.Checklists.Entities;
using Checkmunk.Domain.Checklists.ValueObjects;

namespace Checkmunk.Domain.Checklists.AggregateRoots
{
    public interface IChecklistTitle
    {
        IChecklistCreatedByUser WithTitle(string title);
    }

    public interface IChecklistCreatedByUser
    {
        IChecklistOptionalValues ByUser(User user);
    }

    public interface IChecklistOptionalValues
    {
        IChecklistOptionalValues OnDate(DateTime onDate);

        IChecklistOptionalValues WithItems(IList<ChecklistItem> items);

        Checklist Finish();
    }
}
