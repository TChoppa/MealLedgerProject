using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;

namespace MealLedger.Services
{
    public class KeepAliveService:BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<KeepAliveService> _logger;
        private readonly IConfiguration _config;

        public KeepAliveService(
            IHttpClientFactory httpClientFactory,
            ILogger<KeepAliveService> logger,
            IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _config = config;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("KeepAliveService started.");

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var appUrl = _config["AppSettings:AppUrl"];

                    if (!string.IsNullOrEmpty(appUrl))
                    {
                        var client = _httpClientFactory.CreateClient();
                        var response = await client.GetAsync(appUrl, stoppingToken);

                        _logger.LogInformation(
                            "Keep alive ping sent. Status: {status}",
                            response.StatusCode);
                    }
                }
                catch (TaskCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "KeepAlive ping failed.");
                }

#if DEBUG
                // ── TESTING: Ping every 1 minute ──
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
#else
            // ── PRODUCTION: Ping every 20 minutes ──
            await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
#endif
            }

            _logger.LogInformation("KeepAliveService stopped.");
        }


        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("KeepAliveService started.");

        //    // Wait 1 minute after app starts before first ping
        //    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            var appUrl = _config["AppSettings:AppUrl"];

        //            if (!string.IsNullOrEmpty(appUrl))
        //            {
        //                var client = _httpClientFactory.CreateClient();
        //                var response = await client.GetAsync(appUrl, stoppingToken);

        //                _logger.LogInformation(
        //                    "Keep alive ping sent to {url}. Status: {status}",
        //                    appUrl,
        //                    response.StatusCode);
        //            }
        //        }
        //        catch (TaskCanceledException)
        //        {
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "KeepAlive ping failed.");
        //        }

        //        // ── Ping every 20 minutes ──
        //        await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
        //    }

        //    _logger.LogInformation("KeepAliveService stopped.");
        //}
    }
}
