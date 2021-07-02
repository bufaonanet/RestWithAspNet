using RestWithAspNet.Data.DTO;
using System.Collections.Generic;

namespace RestWithAspNet.Business
{
    public interface IPersonBusiness 
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindByID(long id);
        List<PersonDTO> FindAll();
        List<PersonDTO> FindPersonByName(string firstName, string lastName);
        PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int  pageSize, int page);      
        PersonDTO Update(PersonDTO person);
        PersonDTO Disabled(long id);
        void Delete(long id);
    }
}
