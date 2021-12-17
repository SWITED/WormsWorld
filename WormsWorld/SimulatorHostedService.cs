using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WormsWorld.service.@interface;

namespace WormsWorld
{
    public class SimulatorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<SimulatorHostedService> _logger;
        private readonly ISimulator _simulator;

        public SimulatorHostedService(ISimulator simulator, IHostApplicationLifetime appLifetime,
            ILogger<SimulatorHostedService> logger)
        {
            _appLifetime = appLifetime;
            _logger = logger;
            _simulator = simulator;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(RunAsync, cancellationToken);
            return Task.CompletedTask;
        }

        private void RunAsync()
        {
            Thread.Sleep(250);
            try
            {
                _simulator.Start();
            }
            catch (Exception e)
            {
                _logger.LogError("Simulator stopped with exception: ${e}", e);
                throw;
            }
            finally
            {
                _appLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}