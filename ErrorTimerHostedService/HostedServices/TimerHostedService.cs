using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ErrorTimerHostedService.HostedServices
{
    public class TimerHostedService : IHostedService
    {
        private int counter;
        private Timer timer;
        private readonly ILogger<TimerHostedService> logger;
        public TimerHostedService(ILogger<TimerHostedService> logger)
        {
            this.logger = logger;
            this.counter = 0;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning($"TEST StartAsync: Start NOW. COUNTUP:{counter},NOW:{DateTime.Now.ToLongTimeString()}");
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
                                counter++;
                                logger.LogWarning($"TEST TimerCallback: LOGGING WARNING. COUNTUP:{counter},NOW:{DateTime.Now.ToLongTimeString()}");

                                if (counter >= 10)
                                    throw;
                            }
                        }),
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning($"TEST StopAsync: NOW:{DateTime.Now.ToLongTimeString()}");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
