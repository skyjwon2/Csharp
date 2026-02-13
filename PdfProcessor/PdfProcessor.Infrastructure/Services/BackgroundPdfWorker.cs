using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PdfProcessor.Application.Interfaces;
using PdfProcessor.Domain;

namespace PdfProcessor.Infrastructure.Services
{
    public class BackgroundPdfWorker : BackgroundService
    {
        private readonly Channel<ProcessingJob> _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundPdfWorker> _logger;

        public BackgroundPdfWorker(IServiceProvider serviceProvider, ILogger<BackgroundPdfWorker> logger)
        {
            _channel = Channel.CreateUnbounded<ProcessingJob>();
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task QueueJobAsync(Guid jobId, string filePath, string originalName)
        {
            await _channel.Writer.WriteAsync(new ProcessingJob(jobId, filePath, originalName));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var job in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var analysisService = scope.ServiceProvider.GetRequiredService<IPdfAnalysisService>();
                        
                        _logger.LogInformation($"Starting processing job {job.JobId}");

                        // Load file from path (In real app, use IStorageService to read)
                        using (var fileStream = File.OpenRead(job.FilePath))
                        {
                            var result = await analysisService.AnalyzePdfAsync(fileStream, job.OriginalName);
                            // Save result to DB (Stub)
                            _logger.LogInformation($"Completed job {job.JobId}. Extracts: {result.TableOfContents.Count} TOC items.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing job {job.JobId}");
                }
            }
        }

        public record ProcessingJob(Guid JobId, string FilePath, string OriginalName);
    }
}
