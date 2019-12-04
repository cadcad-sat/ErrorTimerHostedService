using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ErrorTimerHostedService.HostedServices
{
    public class TimerHostedService : IHostedService
    {
        private Timer timer;
        private readonly ILogger<TimerHostedService> logger;
        public TimerHostedService(ILogger<TimerHostedService> logger)
        {
            this.logger = logger;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(
                    callback: new TimerCallback(
                        (x) =>
                        {
                            try
                            {
                                throw new Exception("TEST THROWS!!");
                            }
                            catch
                            {
                                logger.LogWarning($"TEST: LOGGING WARNING. {DateTime.Now.ToLongTimeString()}");
                            }
                        }),
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromSeconds(20));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
