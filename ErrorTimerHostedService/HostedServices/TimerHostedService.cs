using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ErrorTimerHostedService.HostedServices
{
    public class TimerHostedService : IHostedService
    {
        private Timer timer;
        public TimerHostedService() { }
        public void Dispose()
        {
            timer?.Dispose();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(
                    callback: new TimerCallback(
                        async (x) =>
                        {
                            await Task.Delay(15000);
                            throw new Exception("TEST THROWS!!");
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
