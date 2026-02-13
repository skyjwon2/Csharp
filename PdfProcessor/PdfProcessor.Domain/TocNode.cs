using System.Collections.Generic;

namespace PdfProcessor.Domain
{
    public class TocNode
    {
        public required string Title { get; set; }
        public int PageNumber { get; set; }
        public List<TocNode> Children { get; set; } = new List<TocNode>();

        // Heuristics metadata (optional, for debugging/adjustment)
        public double AverageFontSize { get; set; }
        public bool IsBold { get; set; }
    }
}
