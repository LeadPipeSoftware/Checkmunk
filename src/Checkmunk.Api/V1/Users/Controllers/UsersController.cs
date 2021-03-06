﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.WebApi;
using Checkmunk.Application.Common;
using Checkmunk.Application.Users.Commands.CreateUser;
using Checkmunk.Application.Users.Commands.DeleteUser;
using Checkmunk.Application.Users.Commands.UpdateUser;
using Checkmunk.Application.Users.Queries.AllUsers;
using Checkmunk.Application.Users.Queries.UserByEmailAddress;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Domain.Users;
using LeadPipe.Net.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Api.V1.Users.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> logger;
        private readonly IMediator mediator;

        public UsersController(ILogger<UsersController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

		// GET api/v1/users
		/// <summary>
		/// Get all users.
		/// </summary>
		/// <returns>An array of users.</returns>
		/// <response code="200">Success.</response>
		/// <response code="500">Server error.</response>
		[AuditApi]
        [Produces("application/json", Type = typeof(UserModel[]))]
		[ProducesResponseType(typeof(UserModel[]), 200)]
		[ProducesResponseType(typeof(void), 500)]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var response = await mediator.Send<UserModel[]>(new AllUsersQuery(Guid.NewGuid()));

            if (response == null)
            {
                return new ObjectResult(new UserModel[] { });
            }

			return new ObjectResult(response);
		}

		// GET api/v1/users/theon@greyjoy.org
		/// <summary>
		/// Get a specific user by email address.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="emailAddress">The user's email address.</param>
		/// <response code="200">Success.</response>
		/// <response code="400">Email address invalid.</response>
		/// <response code="404">User not found.</response>
		/// <response code="500">Server error.</response>
		[AuditApi]
		[Produces("application/json", Type = typeof(UserModel))]
		[ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(typeof(void), 404)]
		[ProducesResponseType(typeof(void), 500)]
		[HttpGet("{emailAddress}", Name = "GetUserByEmailAddress")]
		public async Task<IActionResult> Get(string emailAddress)
		{
			if (emailAddress.IsNullOrEmpty()) return BadRequest();

			var response = await mediator.Send<UserModel>(new UserByEmailAddressQuery(Guid.NewGuid(), emailAddress));

            if (response == null) return NotFound();

            return new ObjectResult(response);
		}

		// POST api/v1/users
		/// <summary>
		/// Create a user.
		/// </summary>
		/// <param name="user">The user to create.</param>
		/// <response code="201">User created.</response>
		/// <response code="400">User invalid.</response>
		/// <response code="409">User already exists.</response>
		/// <response code="500">Server error.</response>
		[AuditApi]
		[HttpPost]
		[ProducesResponseType(typeof(UserModel), 201)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(typeof(void), 409)]
		[ProducesResponseType(typeof(void), 500)]
		public async Task<IActionResult> Post([FromBody]CreateUserModel user)
		{
            if (user == null)
            {
                return BadRequest();
            }

		    var correlationGuid = Guid.NewGuid();

            UserModel response;

            try
            {
                var existingUser = await mediator.Send<UserModel>(new UserByEmailAddressQuery(correlationGuid, user.EmailAddress));

                if (existingUser != null) return StatusCode(409);

                logger.LogWarning(LoggingEvents.CREATE_USER, "A POST request was received for {EmailAddress}, but that user already exists", user.EmailAddress);

                response = await mediator.Send<UserModel>(new CreateUserCommand(correlationGuid, user));
            }
            catch (Exception e)
            {
                logger.LogError(LoggingEvents.CREATE_USER, e, e.Message);

                return StatusCode(500);
            }

            return StatusCode(201);

            //return CreatedAtRoute("GetUserByEmailAddress", new { id = response.EmailAddress }, response);
        }

		// PUT api/v1/users/theon@greyjoy.com
		/// <summary>
		/// Update the specified user.
		/// </summary>
		/// <param name="emailAddress">The user's email address.</param>
		/// <param name="user">User.</param>
		/// <response code="204">The user was successfully updated.</response>
		/// <response code="400">Email address invalid.</response>
		/// <response code="404">User not found.</response>
		/// <response code="500">Server error.</response>
		[AuditApi]
		[HttpPut("{emailAddress}")]
		[ProducesResponseType(typeof(void), 204)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(typeof(void), 404)]
		[ProducesResponseType(typeof(void), 500)]
		public async Task<IActionResult> Put(string emailAddress, [FromBody]UpdateUserModel user)
		{
            if (emailAddress.IsNullOrEmpty()) return BadRequest();

		    var correlationGuid = Guid.NewGuid();

			var existingUser = await mediator.Send<UserModel>(new UserByEmailAddressQuery(correlationGuid, emailAddress));

		    if (existingUser == null)
		    {
                logger.LogWarning(LoggingEvents.UPDATE_USER, "A request to update a user that does not exist ({EmailAddress}) was received", emailAddress);

		        return NotFound();
		    }

			try
			{
                await mediator.Send(new UpdateUserCommand(correlationGuid, emailAddress, user));
			}
			catch (Exception e)
			{
                logger.LogError(LoggingEvents.UPDATE_USER, e, e.Message);

				return StatusCode(500);
			}

			return new NoContentResult();
		}

		// DELETE api/v1/users/theon@greyjoy.com
		/// <summary>
		/// Delete the specified user.
		/// </summary>
		/// <param name="emailAddress">The user's email address.</param>
		/// <response code="204">The user was successfully deleted.</response>
		/// <response code="400">Email address invalid.</response>
		/// <response code="404">User not found.</response>
		/// <response code="500">Server error.</response>
		[HttpDelete("{emailAddress}")]
		[ProducesResponseType(typeof(void), 204)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(typeof(void), 404)]
		[ProducesResponseType(typeof(void), 500)]
		public async Task<IActionResult> Delete(string emailAddress)
		{
			if (emailAddress.IsNullOrEmpty()) return BadRequest();

		    var correlationGuid = Guid.NewGuid();

			var existingUser = await mediator.Send<UserModel>(new UserByEmailAddressQuery(correlationGuid, emailAddress));

			if (existingUser == null)
            {
                logger.LogWarning(LoggingEvents.DELETE_USER, "A request to delete a user that does not exist ({EmailAddress}) was received", emailAddress);

                return NotFound();
            }

			try
			{
                await mediator.Send(new DeleteUserCommand(correlationGuid, emailAddress));
			}
			catch (Exception e)
			{
                logger.LogError(LoggingEvents.DELETE_USER, e, e.Message);

				return StatusCode(500);
			}

			return new NoContentResult();
		}
	}
}
