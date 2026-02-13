using System.IO;
using System.Threading.Tasks;

namespace PdfProcessor.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveImageAsync(byte[] imageBytes, string imageName);
    }
}
