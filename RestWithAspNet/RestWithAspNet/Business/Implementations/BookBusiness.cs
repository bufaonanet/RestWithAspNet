using RestWithAspNet.Data.Converter;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Model;
using RestWithAspNet.Repository.Generic;
using System.Collections.Generic;

namespace RestWithAspNet.Business.Implementations
{
    public class BookBusiness : IBookBusiness
    {
        private readonly IRepository<Book> _repository;
        private readonly BookConverter _converter;

        public BookBusiness(IRepository<Book> repository)
        {
            _repository = repository;
            _converter = new BookConverter();
        }

        public List<BookDTO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public BookDTO FindById(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public BookDTO Create(BookDTO bookDTO)
        {
            var book = _converter.Parse(bookDTO);
            var bookEntity = _repository.Create(book);
            return _converter.Parse(bookEntity);
        }

        public BookDTO Update(BookDTO bookDTO)
        {
            var book = _converter.Parse(bookDTO);
            var bookEntity = _repository.Update(book);
            return _converter.Parse(bookEntity);
        }
        public void Delete(long id)
        {
            _repository.Delete(id);
        }        
    }
}
