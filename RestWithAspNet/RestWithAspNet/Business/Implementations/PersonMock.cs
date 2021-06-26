using RestWithAspNet.Data.DTO;
using System.Collections.Generic;
using System.Threading;

namespace RestWithAspNet.Business.Implementations
{
    public class PersonMock : IPersonBusiness
    {
        private volatile int count;

        public PersonDTO Create(PersonDTO person)
        {
            return person;
        }

        public void Delete(long id)
        {

        }

        public List<PersonDTO> FindAll()
        {
            List<PersonDTO> people = new List<PersonDTO>();

            for (int i = 0; i < 8; i++)
            {
                PersonDTO person = MockPerson(count);
                people.Add(person);
            }

            return people;
        }

        public PersonDTO FindByID(long id)
        {
            return new PersonDTO
            {
                Id = InrementeAndGet(),
                FirstName = "Douglas",
                LastName = "Souto de Souza",
                Address = "Guanhães/MG",
                Gender = "Male"
            };
        }

        public PersonDTO Update(PersonDTO person)
        {
            return person;
        }

        private PersonDTO MockPerson(int i)
        {
            return new PersonDTO
            {
                Id = InrementeAndGet(),
                FirstName = "Person Name" + i,
                LastName = "Person Last Name" + i,
                Address = "Some Adddress" + i,
                Gender = "Male"
            };
        }

        private long InrementeAndGet()
        {
            return Interlocked.Increment(ref count);
        }
    }
}
