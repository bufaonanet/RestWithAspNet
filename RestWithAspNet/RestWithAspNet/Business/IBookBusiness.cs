using RestWithAspNet.Data.DTO;
using System.Collections.Generic;

namespace RestWithAspNet.Business
{
    public interface IBookBusiness
    {
        BookDTO Create(BookDTO book);
        BookDTO FindById(long id);
        List<BookDTO> FindAll();
        BookDTO Update(BookDTO book);
        void Delete(long id);
    }
}
