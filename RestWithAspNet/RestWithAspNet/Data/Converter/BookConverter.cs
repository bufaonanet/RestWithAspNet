using RestWithAspNet.Data.Converter.Base;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Model;
using System.Collections.Generic;
using System.Linq;

namespace RestWithAspNet.Data.Converter
{
    public class BookConverter : IConverter<Model.Book, DTO.BookDTO>, IConverter<DTO.BookDTO, Model.Book>
    {
        public DTO.BookDTO Parse(Model.Book origin)
        {
            if (origin == null) return null;

            return new DTO.BookDTO
            {
                Id = origin.Id,
                Title = origin.Title,
                Author = origin.Author,
                Price = origin.Price,
                LaunchDate = origin.LaunchDate               
            };
        }

        public Model.Book Parse(DTO.BookDTO origin)
        {
            if (origin == null) return null;

            return new Model.Book
            {
                Id = origin.Id,
                Title = origin.Title,
                Author = origin.Author,
                Price = origin.Price,
                LaunchDate = origin.LaunchDate
            };
        }

        public List<DTO.BookDTO> Parse(List<Model.Book> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<Model.Book> Parse(List<DTO.BookDTO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }


}
