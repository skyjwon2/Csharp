using System;

namespace PdfProcessor.Domain
{
    public class ExtractedImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string FilePath { get; set; }
        public int PageNumber { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public required string Format { get; set; } // e.g., "png", "jpg"
    }
}
