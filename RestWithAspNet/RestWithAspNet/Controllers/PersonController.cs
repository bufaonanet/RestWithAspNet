using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithAspNet.Model;
using RestWithAspNet.Business;
using RestWithAspNet.Data.DTO;

namespace RestWithAspNet.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonController : ControllerBase
    {        
        private readonly IPersonBusiness _personBusiness;

        public PersonController( IPersonBusiness personBusiness)
        {            
            _personBusiness = personBusiness;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var person = _personBusiness.FindByID(id);

            if (person == null) return NotFound();

            return Ok(person);
        }

        [HttpPost()]
        public IActionResult Post(PersonDTO person)
        {
            if (person == null) return BadRequest();

            return Ok(_personBusiness.Create(person));
        }

        [HttpPut()]
        public IActionResult Put(PersonDTO person)
        {
            if (person == null) return BadRequest();

            return Ok(_personBusiness.Update(person));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }


    }
}
