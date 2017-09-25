using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkmunk.Domain.Checklists.AggregateRoots
{
    public interface IChecklistRepository
    {
        /// <summary>
        /// Gets all checklists.
        /// </summary>
        /// <returns>All checklists.</returns>
        Task<Checklist[]> GetAllChecklists();

        /// <summary>
        /// Gets a checklist based on its id.
        /// </summary>
        /// <param name="id">The id of the checklist.</param>
        /// <returns>The matching checklist.</returns>
        Task<Checklist> GetChecklistById(Guid id);
    }
}