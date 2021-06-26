using RestWithAspNet.Data.DTO;
using System.Collections.Generic;

namespace RestWithAspNet.Business
{
    public interface IPersonBusiness 
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindByID(long id);
        List<PersonDTO> FindAll();
        PersonDTO Update(PersonDTO person);
        void Delete(long id);
    }
}
