using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication16.Services
{
    public interface IFileStorageService
    {
        Task<string?> StoreFileAsync(IFormFile file, string folderName);
        void DeleteFile(string filePath);
    }
}


//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;

//namespace WebApplication16.Services
//{
//    public interface IFileStorageService
//    {
//        Task<string?> StoreFileAsync(IFormFile file, string folderName);
//        void DeleteFile(string filePath);
//    }
//}
