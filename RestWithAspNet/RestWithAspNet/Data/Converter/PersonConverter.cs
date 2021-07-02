using RestWithAspNet.Data.Converter.Base;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithAspNet.Data.Converter
{
    public class PersonConverter : IConverter<Person, PersonDTO>, IConverter<PersonDTO, Person>
    {
        public PersonDTO Parse(Person origin)
        {
            if (origin == null) return null;

            return new PersonDTO
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender,
                Enabled = origin.Enabled
            };
        }

        public Person Parse(PersonDTO origin)
        {
            if (origin == null) return null;

            return new Person
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender,
                Enabled = origin.Enabled
            };
        }

        public List<PersonDTO> Parse(List<Person> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<Person> Parse(List<PersonDTO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }


}
