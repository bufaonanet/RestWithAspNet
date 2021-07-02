using Microsoft.Data.SqlClient;
using RestWithAspNet.Data.Converter;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Repository;
using System.Collections.Generic;

namespace RestWithAspNet.Business.Implementations
{
    public class PersonBusiness : IPersonBusiness
    {
        private readonly IPersonRepository _repository;
        private readonly PersonConverter _converter;

        public PersonBusiness(IPersonRepository context)
        {
            _repository = context;
            _converter = new PersonConverter();
        }

        public List<PersonDTO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PersonDTO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public List<PersonDTO> FindPersonByName(string firstName, string lastName)
        {
            return _converter.Parse(_repository.FindPersonByName(firstName, lastName));
        }

        public PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Contains("desc")) ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offSet = (page > 0) ? (page - 1) * size : 0;

            //var query = @"select * from person p where 1 = 1 ";
            //if (!string.IsNullOrWhiteSpace(name)) query += $" and p.first_name like %{name}% ";
            //query += $" order by p.first_name {sort} OFFSET {offSet} ROWS FETCH FIRST {size} ROWS ONLY ";

            //string countQuery = @"select count(*) from person p where 1 = 1 ";
            //if (!string.IsNullOrWhiteSpace(name)) countQuery += $" and p.first_name like %{name}% ";


            //var person = _repository.FindWithPagedSearch(query);
            var person = _repository.FindWithPagedSearch(name, sort, size, offSet);

            int totalResults = 0;
            if (!string.IsNullOrWhiteSpace(name))
                totalResults = _repository.CountTotal(p => p.FirstName.Contains(name));
            else
                totalResults = _repository.CountTotal(p => true);

            return new PagedSearchDTO<PersonDTO>
            {
                CurrentPage = page,
                List = _converter.Parse(person),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public PersonDTO Create(PersonDTO personDTO)
        {
            var person = _converter.Parse(personDTO);
            var personEntity = _repository.Create(person);
            return _converter.Parse(personEntity);
        }

        public PersonDTO Update(PersonDTO personDTO)
        {
            var person = _converter.Parse(personDTO);
            var personEntity = _repository.Update(person);
            return _converter.Parse(personEntity);
        }

        public PersonDTO Disabled(long id)
        {
            var personEntity = _repository.Disabled(id);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }


    }
}
