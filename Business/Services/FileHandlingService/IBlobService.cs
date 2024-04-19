using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.FileHandlingServices
{
    public interface IBlobService
    {
        Task<bool> DeleteBlob(string blobName, string containerName);
        Task<string> GetBlob(string blobName, string containerName);
        Task<string> UploadBlob(string blobName, string containerName, IFormFile file);
    }
}
