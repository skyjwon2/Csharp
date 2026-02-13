using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PdfProcessor.Application.Interfaces;
using PdfProcessor.Domain;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;


namespace PdfProcessor.Infrastructure.Services
{
    public class PdfPigAnalysisService : IPdfAnalysisService
    {
        private readonly IStorageService _storageService;

        public PdfPigAnalysisService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<PdfDocumentModel> AnalyzePdfAsync(Stream pdfStream, string originalFileName)
        {
            var documentModel = new PdfDocumentModel
            {
                Id = Guid.NewGuid(),
                OriginalFileName = originalFileName,
                UploadedAt = DateTime.UtcNow,
                Status = ProcessingStatus.Processing
            };

            // Using stream directly (PdfPig supports stream)
            // Note: PdfDocument needs to be disposed.
            using (var document = PdfDocument.Open(pdfStream))
            {
                var processedLines = new List<TextLine>();

                // 1. Text & Hierarchy Analysis
                foreach (var page in document.GetPages())
                {
                    // Reconstruct Lines
                    var linesOnPage = GetLinesFromPage(page);
                    processedLines.AddRange(linesOnPage);

                    // Image Extraction (basic implementation)
                    foreach (var image in page.GetImages())
                    {
                        if (image.TryGetPng(out var pngBytes))
                        {
                            var imageName = $"{documentModel.Id}_p{page.Number}_{Guid.NewGuid()}.png";
                            var storedPath = await _storageService.SaveImageAsync(pngBytes, imageName);
                            
                            documentModel.Images.Add(new ExtractedImage
                            {
                                FilePath = storedPath,
                                PageNumber = page.Number,
                                Width = image.Bounds.Width, // Using available bounds
                                Height = image.Bounds.Height,
                                Format = "png"
                            });
                        }
                    }
                }

                // 2. Build TOC Hierarchy based on heuristics
                documentModel.TableOfContents = BuildTableOfContents(processedLines);
                
                // 3. Simple text aggregation
                documentModel.TextContent = string.Join("\n", processedLines.Select(l => l.Text));
                documentModel.Status = ProcessingStatus.Completed;
            }

            return documentModel;
        }

        private List<TextLine> GetLinesFromPage(Page page)
        {
            var words = page.GetWords().ToList();
            if (!words.Any()) return new List<TextLine>();

            // Group words into lines based on Y-coordinate proximity (Tolerance +/- 2.0)
            // PDF Y-coordinates start from bottom-left. Higher Y = Top of page.
            // We want to sort lines from Top to Bottom (Descending Y).
            
            var lines = new List<TextLine>();
            
            // Simple clustering: Sort by Y descending first
            var sortedWords = words.OrderByDescending(w => w.BoundingBox.Bottom).ToList();
            
            var currentLineWords = new List<Word>();
            double currentLineY = sortedWords.First().BoundingBox.Bottom;

            foreach (var word in sortedWords)
            {
                if (Math.Abs(word.BoundingBox.Bottom - currentLineY) < 3.0) // Tolerance
                {
                    currentLineWords.Add(word);
                }
                else
                {
                    // Finish verify previous line
                    if (currentLineWords.Any())
                    {
                        lines.Add(CreateLine(currentLineWords, page.Number));
                    }
                    
                    // Start new line
                    currentLineWords = new List<Word> { word };
                    currentLineY = word.BoundingBox.Bottom;
                }
            }
            
            // Add the last line
            if (currentLineWords.Any())
            {
                lines.Add(CreateLine(currentLineWords, page.Number));
            }

            return lines;
        }

        private TextLine CreateLine(List<Word> words, int pageNumber)
        {
            // Sort words in line from Left to Right (Ascending X)
            var sortedLine = words.OrderBy(w => w.BoundingBox.Left).ToList();
            var text = string.Join(" ", sortedLine.Select(w => w.Text));
            
            // Calculate heuristic metadata
            var maxFontSize = sortedLine.Max(w => w.Letters[0].PointSize);
            // Check for bold font in any word (simplification)
            var isBold = sortedLine.Any(w => w.Letters[0].FontName.Contains("Bold", StringComparison.OrdinalIgnoreCase));
            var indent = sortedLine.First().BoundingBox.Left;

            return new TextLine
            {
                Text = text,
                PageNumber = pageNumber,
                FontSize = maxFontSize,
                IsBold = isBold,
                Indentation = indent,
                Details = sortedLine.First() // Store first word for details if needed
            };
        }

        private List<TocNode> BuildTableOfContents(List<TextLine> lines)
        {
            // Simple heuristic to build a tree
            // 1. Calculate median font size
            if (!lines.Any()) return new List<TocNode>();
            
            double globalAverageFontSize = lines.Average(l => l.FontSize);
            // Threshold for headings: significantly larger or Bold
            double headingThreshold = globalAverageFontSize * 1.2; 

            var rootNodes = new List<TocNode>();
            var stack = new Stack<TocNode>(); // To track hierarchy

            foreach (var line in lines)
            {
                bool isHeading = line.FontSize > headingThreshold || (line.IsBold && line.FontSize >= globalAverageFontSize);
                
                if (isHeading)
                {
                    var newNode = new TocNode
                    {
                        Title = line.Text,
                        PageNumber = line.PageNumber,
                        AverageFontSize = line.FontSize,
                        IsBold = line.IsBold
                    };

                    // Logic to determine nesting based on font size/indent
                    // For now, simpler logic:
                    // If stack is empty, it's a root node.
                    // If new node is "smaller" (less importance) than top of stack, it's a child.
                    // If new node is "bigger" or equal, we pop until we find a parent or become root.
                    
                    // Note: In PDFs, H1 usually strictly larger than H2.
                    // If strictly smaller font size -> Child
                    // If same or larger -> Sibling or Parent's Sibling
                    
                    while (stack.Count > 0)
                    {
                        var parent = stack.Peek();
                        // If current node is smaller font size, it is a child of the parent
                        if (newNode.AverageFontSize < parent.AverageFontSize - 1.0) 
                        {
                            parent.Children.Add(newNode);
                            stack.Push(newNode);
                            goto NextLine; // Effectively break inner loop and continue outer
                        }
                        else
                        {
                            // Current node is same level or higher level, pop the stack
                            stack.Pop();
                        }
                    }

                    // If stack is empty, it's a root node
                    if (stack.Count == 0)
                    {
                        rootNodes.Add(newNode);
                        stack.Push(newNode);
                    }
                }

                NextLine:;
            }

            return rootNodes;
        }

        // Helper class for processing
        private class TextLine
        {
            public required string Text { get; set; }
            public int PageNumber { get; set; }
            public double FontSize { get; set; }
            public bool IsBold { get; set; }
            public double Indentation { get; set; }
            public required Word Details { get; set; }
        }
    }
}
