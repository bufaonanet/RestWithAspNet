using Microsoft.AspNetCore.Http;
using RestWithAspNet.Data.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithAspNet.Business.Implementations
{
    public class FileBusiness : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileBusiness(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Directory.GetCurrentDirectory() + @"\UpLoadDir\";
        }

        public byte[] GetFile(string fileName)
        {
            var filepath = _basePath + fileName;
            return File.ReadAllBytes(filepath);
        }
        public async Task<FileDetailDTO> SaveFileToDisckAsync(IFormFile file)
        {
            var fileDetailDTO = new FileDetailDTO();

            var fileType = Path.GetExtension(file.FileName);
            var baseUrl = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" ||
                fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetailDTO.DocName = docName;
                    fileDetailDTO.DocType = fileType;
                    fileDetailDTO.DocUrl = Path.Combine(baseUrl + "/api/v1.0/file/" + fileDetailDTO.DocName);

                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetailDTO;
        }

        public async Task<List<FileDetailDTO>> SaveFilesToDisckAsync(List<IFormFile> files)
        {
            var listFiles = new List<FileDetailDTO>();

            foreach (var file in files)
            {
                listFiles.Add(await SaveFileToDisckAsync(file));
            }

            return listFiles;
        }


    }
}
