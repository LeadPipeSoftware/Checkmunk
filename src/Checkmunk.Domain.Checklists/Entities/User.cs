using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Common;
using LeadPipe.Net;
using System.Collections.Generic;

namespace Checkmunk.Domain.Checklists.Entities
{
    public class User : IAmPersistable
    {
        private IList<Checklist> checklists;
        private string emailAddress;

        public User(string emailAddress)
        {
            Guard.Will.ProtectAgainstNullOrEmptyStringArgument(() => emailAddress);

            this.emailAddress = emailAddress;

            this.checklists = new List<Checklist>();
        }

        protected internal User()
        {
            // For ORM use
        }

        public virtual int PersistenceId { get; set; }

        public virtual IList<Checklist> Checklists
        {
            get => checklists;
            protected set => checklists = value;
        }

        public virtual string EmailAddress
        {
            get => emailAddress;
            protected set => emailAddress = value;
        }
    }
}