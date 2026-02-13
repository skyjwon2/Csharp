using System;
using System.Collections.Generic;

namespace PdfProcessor.Domain
{
    public class PdfDocumentModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string OriginalFileName { get; set; }
        public string? StoredFileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public ProcessingStatus Status { get; set; } = ProcessingStatus.Pending;
        public string? TextContent { get; set; }
        public List<TocNode> TableOfContents { get; set; } = new();
        public List<ExtractedImage> Images { get; set; } = new();

        public int Count => 3;
        public int Count2 { get {return 3;}}
        public int Count3 { get ; private set; }
    }

    public enum ProcessingStatus
    {
        Pending,
        Processing,
        Completed,
        Failed
    }
}
