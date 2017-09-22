using System;

namespace Checkmunk.Domain.Common
{
    /*
     * Persistence is a leaky abstraction. Ideally, our domain is ignorant of
     * the fact that it can be saved somewhere or retrieved. However, it's not
     * terribly practical to design a domain object that way. The fact remains
     * that databases often need identifiers.
     * 
     * At the very least, I try to make sure that the notion of identity is
     * separate when it comes to domain (business) versus data (infrastructure)
     * and so I'll use terms like 'Id' for the domain and 'PersistenceId' for
     * data concerns.
     */

    public interface IAmPersistable
    {
        int PersistenceId { get; set; }
    }
}
