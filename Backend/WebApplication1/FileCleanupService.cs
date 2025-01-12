using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryBackend
{
    public class FileCleanupService : IHostedService
    {
        private readonly ILogger<FileCleanupService> _logger;

        private readonly IHostApplicationLifetime _lifetime;
        private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public FileCleanupService(IHostApplicationLifetime lifetime, ILogger<FileCleanupService> logger)
        {
            _lifetime = lifetime;
            _logger = logger;
        }

        /// <summary>
        /// Function that defines what our method does at the start of the program
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>When the program starts we dont need to do anything so we just return</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            CleanUpImageFolder();
            return Task.CompletedTask;
        }

        private void CleanUpImageFolder()
        {
            try
            {
                if (Directory.Exists(_imageDirectory))
                {
                    var files = Directory.GetFiles(_imageDirectory);
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                            _logger.LogInformation($"Deleted file: {file}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error deleting file: {file}, error code: {ex}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cleaning up image folder {ex.Message}");
            }
        }
    }
}
