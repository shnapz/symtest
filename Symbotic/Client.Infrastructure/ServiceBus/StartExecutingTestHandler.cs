using Contracts.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Client.Infrastructure.ServiceBus
{
    // <summary>
    // Handling event of start executing test external Api.
    // </summary>
    public sealed class StartExecutingTestHandler : IConsumer<IStartExecutingTestEvent>
    {
        private readonly ILogger _logger;

        public StartExecutingTestHandler(ILogger<StartExecutingTestHandler> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IStartExecutingTestEvent> context)
        {
            _logger.LogInformation("Start executing test external Api");

            return Task.FromResult(0);
        }
    }
}