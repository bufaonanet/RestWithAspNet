using Microsoft.AspNetCore.Http;
using RestWithAspNet.Data.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestWithAspNet.Business
{
    public interface IFileBusiness
    {
        public byte[] GetFile(string name);
        public Task<FileDetailDTO> SaveFileToDisckAsync(IFormFile file);
        public Task<List<FileDetailDTO>> SaveFilesToDisckAsync(List<IFormFile> file);
    }
}
