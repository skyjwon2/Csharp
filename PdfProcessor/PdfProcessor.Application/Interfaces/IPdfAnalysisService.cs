using System.IO;
using System.Threading.Tasks;
using PdfProcessor.Domain;

namespace PdfProcessor.Application.Interfaces
{
    public interface IPdfAnalysisService
    {
        Task<PdfDocumentModel> AnalyzePdfAsync(Stream pdfStream, string originalFileName);
    }
}
