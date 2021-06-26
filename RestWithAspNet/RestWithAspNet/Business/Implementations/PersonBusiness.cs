using RestWithAspNet.Data.Converter;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Model;
using RestWithAspNet.Repository;
using RestWithAspNet.Repository.Generic;
using System.Collections.Generic;

namespace RestWithAspNet.Business.Implementations
{
    public class PersonBusiness : IPersonBusiness
    {
        private readonly IRepository<Person> _repository;
        private readonly PersonConverter _converter;

        public PersonBusiness(IRepository<Person> context)
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
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
