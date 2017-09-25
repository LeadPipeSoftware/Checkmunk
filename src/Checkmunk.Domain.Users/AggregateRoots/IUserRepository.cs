using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkmunk.Domain.Users.AggregateRoots
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns>All users.</returns>
        Task<User[]> GetAllUsers();

        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="emailAddress">The user's email address.</param>
        /// <returns>The matching user.</returns>
        Task<User> GetUserByEmailAddress(string emailAddress);
    }
}
