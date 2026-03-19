using MealLedger.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace MealLedger.Services
{
    public class CleanupService : BackgroundService
    {

        private readonly IServiceProvider _services;
        private readonly ILogger<CleanupService> _logger;

        public CleanupService(
            IServiceProvider services,
            ILogger<CleanupService> logger)
        {
            _services = services;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CleanupService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
#if DEBUG
                    // ── TESTING: Run every 2 minutes ──
                    _logger.LogInformation(
                        "DEBUG mode — cleanup runs in 1 minutes.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
#else
                // ── PRODUCTION: Run at 6 AM daily ──
                var now     = DateTime.Now;
                var next6AM = DateTime.Today
                                .AddDays(now.Hour >= 6 ? 1 : 0)
                                .AddHours(6);
                var delay   = next6AM - now;

                _logger.LogInformation(
                    "Next cleanup at: {time}. Waiting {hours} hrs {mins} mins.",
                    next6AM,
                    (int)delay.TotalHours,
                    delay.Minutes);

                await Task.Delay(delay, stoppingToken);
#endif

                    // ── Run cleanup ──
                    await RunCleanupAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CleanupService error.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }

            _logger.LogInformation("CleanupService stopped.");
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("CleanupService started.");

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            // ── Calculate time until next 6 AM ──
        //            var now = DateTime.Now;
        //            var next6AM = DateTime.Today
        //                            .AddDays(now.Hour >= 6 ? 1 : 0)
        //                            .AddHours(6);
        //            var delay = next6AM - now;

        //            _logger.LogInformation(
        //                "Next cleanup at: {time}. Waiting {hours} hrs {mins} mins.",
        //                next6AM,
        //                (int)delay.TotalHours,
        //                delay.Minutes);

        //            // ── Wait until 6 AM ──
        //            await Task.Delay(delay, stoppingToken);

        //            // ── Run cleanup ──
        //            await RunCleanupAsync(stoppingToken);
        //        }
        //        catch (TaskCanceledException)
        //        {
        //            // App shutting down — exit gracefully
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "CleanupService error.");

        //            // Wait 1 hour before retrying if error occurs
        //            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        //        }
        //    }

        //    _logger.LogInformation("CleanupService stopped.");
        //}

        private async Task RunCleanupAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider
                                    .GetRequiredService<AppDbContext>();

            var cutoff = DateTime.Today.AddDays(-14);
            var deleted = await context.LunchRegistrations
                .Where(x => x.LunchDate.Date < cutoff)
                .ExecuteDeleteAsync(stoppingToken);

            _logger.LogInformation(
                "Cleanup done at {time}. {count} records deleted. Cutoff: {cutoff}",
                DateTime.Now,
                deleted,
                cutoff.ToString("dd MMM yyyy"));
        }
    }
}
