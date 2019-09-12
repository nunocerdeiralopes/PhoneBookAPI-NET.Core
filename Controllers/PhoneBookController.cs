using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.BusinessLayer;
using PhoneBookAPI.Entities;
using PhoneBookAPI.Models;
using System.Collections.Generic;

namespace PhoneBookAPI.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize(Roles = Role.Admin, AuthenticationSchemes = "Bearer")]
    public class PhoneBookController : ControllerBase
    {
        private readonly PhoneBookService _phoneBookService;

        public PhoneBookController(PhoneBookService phoneBookService)
        {
            _phoneBookService = phoneBookService;
        }

        /// <summary>
        /// Get the full phone book.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<List<Person>> Get() =>
            _phoneBookService.Get();

        /// <summary>
        /// Get a specific record.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>The record.</returns>
        [HttpGet("{id:length(24)}", Name = "GetPerson")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Person> Get(string id)
        {
            var person = _phoneBookService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        /// <summary>
        /// Create a record.
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /API/v1/
        ///     {
        ///        "FirstName": "Nuno",
	    ///        "LastName": "Lopes",
	    ///        "Email": "email@gmail.com",
	    ///        "Phone": "321312312",
	    ///        "Mobile": "2143142134",
	    ///        "Company": "hackajob",
	    ///        "Title": "Software Engineer"
        ///     }
        ///
        /// </remarks>
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public ActionResult<Person> Create(Person person)
        {
            _phoneBookService.Create(person);

            return CreatedAtRoute("GetPerson", new { id = person.Id.ToString() }, person);
        }

        /// <summary>
        /// Update a record.
        /// </summary>
        /// <param name="id">The id of the record being updated.</param>
        /// <param name="bookIn">The record that will replace.</param>
        /// <returns></returns>
        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Update(string id, Person bookIn)
        {
            var person = _phoneBookService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            _phoneBookService.Update(id, bookIn);

            return NoContent();
        }

        /// <summary>
        /// Delete a record.
        /// </summary>
        /// <param name="id">The Id of the record being deleted.</param>
        /// <returns></returns>
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(string id)
        {
            var person = _phoneBookService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            _phoneBookService.Remove(person.Id);

            return NoContent();
        }
    }
}
