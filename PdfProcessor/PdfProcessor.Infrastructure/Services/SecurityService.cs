using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PdfProcessor.Infrastructure.Services
{
    public class SecurityService
    {
        // %PDF- (5 bytes)
        private static readonly byte[] PdfMagicBytes = { 0x25, 0x50, 0x44, 0x46, 0x2D };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB Limit

        public async Task<(bool IsValid, string Message)> ValidatePdfAsync(Stream fileStream, long fileSize)
        {
            if (fileSize > MaxFileSize)
            {
                return (false, "File size exceeds the limit.");
            }

            if (fileStream.Length < PdfMagicBytes.Length)
            {
                return (false, "File is too small to be a PDF.");
            }

            byte[] buffer = new byte[PdfMagicBytes.Length];
            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }
            
            int bytesRead = await fileStream.ReadAsync(buffer, 0, PdfMagicBytes.Length);

            // Reset position for further processing
            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }

            if (bytesRead < PdfMagicBytes.Length || !buffer.SequenceEqual(PdfMagicBytes))
            {
                return (false, "Invalid file format. Magic bytes do not match PDF signature.");
            }

            return (true, "Valid PDF.");
        }

        public string SanitizeFileName(string originalFileName)
        {
            // Use GUID to prevent path traversal and script injection
            string extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}
