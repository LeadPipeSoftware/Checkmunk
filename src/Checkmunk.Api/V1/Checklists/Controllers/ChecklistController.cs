using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audit.WebApi;
using Checkmunk.Application.Checklists.Commands.CreateChecklist;
using Checkmunk.Application.Checklists.Commands.DeleteChecklist;
using Checkmunk.Application.Checklists.Commands.UpdateChecklist;
using Checkmunk.Application.Checklists.Queries.AllChecklists;
using Checkmunk.Application.Checklists.Queries.ChecklistById;
using Checkmunk.Application.Common;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Domain.Checklists;
using LeadPipe.Net.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Api.V1.Checklists.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChecklistController : Controller
    {
        private readonly ILogger<ChecklistController> logger;
        private readonly IMediator mediator;

        public ChecklistController(ILogger<ChecklistController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

		// GET api/v1/checklists
		/// <summary>
		/// Get all checklists.
		/// </summary>
		/// <returns>An array of checklists.</returns>
		/// <response code="200">Success.</response>
		/// <response code="500">Server error.</response>
		[AuditApi]
        [Produces("application/json", Type = typeof(ChecklistModel[]))]
		[ProducesResponseType(typeof(ChecklistModel[]), 200)]
		[ProducesResponseType(typeof(void), 500)]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var response = await mediator.Send<ChecklistModel[]>(new AllChecklistsQuery());

            if (response == null)
            {
                return new ObjectResult(new ChecklistModel[] { });
            }

			return new ObjectResult(response);
		}

        // GET api/v1/checklists/cc14be8a-9a06-43ff-9abf-8721a2a45b08
        /// <summary>
        /// Get a specific checklist by id.
        /// </summary>
        /// <returns>The checklist.</returns>
        /// <param name="id">The checklist id.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Id invalid.</response>
        /// <response code="404">Checklist not found.</response>
        /// <response code="500">Server error.</response>
        [AuditApi]
        [Produces("application/json", Type = typeof(ChecklistModel))]
        [ProducesResponseType(typeof(ChecklistModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [HttpGet("{id}", Name = "GetChecklistById")]
        public async Task<IActionResult> Get(string id)
        {
            var guid = id.ToGuid();

            if (guid.IsDefaultValue()) return BadRequest();

            var response = await mediator.Send<ChecklistModel>(new ChecklistByIdQuery(guid));

            if (response == null) return NotFound();

            return new ObjectResult(response);
        }

        // POST api/v1/checklists
        /// <summary>
        /// Create a checklist.
        /// </summary>
        /// <param name="checklist">The checklist to create.</param>
        /// <response code="201">Checklist created.</response>
        /// <response code="400">Checklist invalid.</response>
        /// <response code="409">Checklist already exists.</response>
        /// <response code="500">Server error.</response>
        [AuditApi]
        [HttpPost]
        [ProducesResponseType(typeof(ChecklistModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Post([FromBody]CreateChecklistModel checklist)
        {
            if (checklist == null)
            {
                return BadRequest();
            }

            ChecklistModel response;

            try
            {
                response = await mediator.Send<ChecklistModel>(new CreateChecklistCommand(checklist));
            }
            catch (Exception e)
            {
                logger.LogError(LoggingEvents.CREATE_CHECKLIST, e, e.Message);

                return StatusCode(500);
            }

            return StatusCode(201);

            //return CreatedAtRoute("GetChecklistById", new { id = response.Id }, response);
        }

        // PUT api/v1/checklists/cc14be8a-9a06-43ff-9abf-8721a2a45b08
        /// <summary>
        /// Update the specified checklist.
        /// </summary>
        /// <param name="id">The checklist id.</param>
        /// <param name="checklist">Checklist.</param>
        /// <response code="204">The checklist was successfully updated.</response>
        /// <response code="400">Id invalid.</response>
        /// <response code="404">Checklist not found.</response>
        /// <response code="500">Server error.</response>
        [AuditApi]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateChecklistModel checklist)
        {
            var guid = id.ToGuid();

            if (guid.IsDefaultValue()) return BadRequest();

            var existingChecklist = await mediator.Send<ChecklistModel>(new ChecklistByIdQuery(guid));

            if (existingChecklist == null) return NotFound();

            try
            {
                await mediator.Send(new UpdateChecklistCommand(guid, checklist));
            }
            catch (Exception e)
            {
                logger.LogError(LoggingEvents.UPDATE_CHECKLIST, e, e.Message);

                return StatusCode(500);
            }

            return new NoContentResult();
        }

        // DELETE api/v1/checklists/cc14be8a-9a06-43ff-9abf-8721a2a45b08
        /// <summary>
        /// Delete the specified checklist.
        /// </summary>
        /// <param name="id">The checklist id.</param>
        /// <response code="204">The checklist was successfully deleted.</response>
        /// <response code="400">Checklist id invalid.</response>
        /// <response code="404">Checklist not found.</response>
        /// <response code="500">Server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            var guid = id.ToGuid();

            if (guid.IsDefaultValue()) return BadRequest();

            var existingChecklist = await mediator.Send<ChecklistModel>(new ChecklistByIdQuery(guid));

            if (existingChecklist == null) return NotFound();

            try
            {
                await mediator.Send(new DeleteChecklistCommand(guid));
            }
            catch (Exception e)
            {
                logger.LogError(LoggingEvents.DELETE_CHECKLIST, e, e.Message);

                return StatusCode(500);
            }

            return new NoContentResult();
        }
    }
}
