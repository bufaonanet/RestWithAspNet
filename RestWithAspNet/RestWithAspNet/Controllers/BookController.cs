using Microsoft.AspNetCore.Mvc;
using RestWithAspNet.Business;
using RestWithAspNet.Data.DTO;
using System.Collections.Generic;

namespace RestWithAspNet.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookBusiness _bookService;

        public BookController(IBookBusiness bookService)
        {
            _bookService = bookService;
        }

        [HttpGet()]
        [ProducesResponseType((200), Type = typeof(List<BookDTO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Get()
        {
            return Ok(_bookService.FindAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(BookDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            var person = _bookService.FindById(id);

            if (person == null) return NotFound();

            return Ok(person);
        }

        [HttpPost()]
        [ProducesResponseType((200), Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Post(BookDTO bookDTO)
        {
            if (bookDTO == null) return BadRequest();

            return Ok(_bookService.Create(bookDTO));
        }

        [HttpPut()]
        [ProducesResponseType((200), Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Put(BookDTO bookDTO)
        {
            if (bookDTO == null) return BadRequest();

            return Ok(_bookService.Update(bookDTO));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((200), Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(long id)
        {
            _bookService.Delete(id);
            return NoContent();
        }


    }
}
