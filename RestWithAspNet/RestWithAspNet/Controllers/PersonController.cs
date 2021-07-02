using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet.Business;
using RestWithAspNet.Data.DTO;
using System.Collections.Generic;

namespace RestWithAspNet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonBusiness _personBusiness;

        public PersonController(IPersonBusiness personBusiness)
        {
            _personBusiness = personBusiness;
        }

        [HttpGet()]
        [ProducesResponseType((200), Type = typeof(List<PersonDTO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //[TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType((200), Type = typeof(List<PersonDTO>))]       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //[TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(
            [FromQuery] string name, string sortDirection, int pageSize, int page)
        {
            return Ok(_personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, page));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(PersonDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //[TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(int id)
        {
            var person = _personBusiness.FindByID(id);

            if (person == null) return NotFound();

            return Ok(person);
        }

        [HttpGet("findpersonbyname")]
        [ProducesResponseType((200), Type = typeof(PersonDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult FindPersonByName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var person = _personBusiness.FindPersonByName(firstName, lastName);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPost()]
        [ProducesResponseType((200), Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        //[TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post(PersonDTO person)
        {
            if (person == null) return BadRequest();

            return Ok(_personBusiness.Create(person));
        }

        [HttpPut()]
        [ProducesResponseType((200), Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        //[TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put(PersonDTO person)
        {
            if (person == null) return BadRequest();

            return Ok(_personBusiness.Update(person));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Patch(long id)
        {
            var person = _personBusiness.Disabled(id);
            return Ok(person);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }


    }
}
