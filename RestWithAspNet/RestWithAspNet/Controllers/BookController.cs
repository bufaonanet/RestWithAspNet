using Microsoft.AspNetCore.Mvc;
using RestWithAspNet.Model;
using RestWithAspNet.Business;

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
        public IActionResult Get()
        {
            return Ok(_bookService.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var person = _bookService.FindById(id);

            if (person == null) return NotFound();

            return Ok(person);
        }

        [HttpPost()]
        public IActionResult Post(Book book)
        {
            if (book == null) return BadRequest();

            return Ok(_bookService.Create(book));
        }

        [HttpPut()]
        public IActionResult Put(Book book)
        {
            if (book == null) return BadRequest();

            return Ok(_bookService.Update(book));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _bookService.Delete(id);
            return NoContent();
        }


    }
}
