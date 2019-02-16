using Contracts.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TasksGenerator.Infrastructure.ListenerExternal;

namespace TasksGenerator.Infrastructure.ServiceBus
{
    /// <summary>
    /// Getting tasks and start task executing
    /// </summary>
    internal sealed class TasksHandler : IConsumer<ITaskCommand>
    {
        private readonly IListenerExternalApi _listenerExternalApi;
        private readonly ILogger _logger;

        public TasksHandler(IListenerExternalApi listenerExternalApi, ILogger<TasksHandler> logger)
        {
            _listenerExternalApi = listenerExternalApi;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ITaskCommand> context)
        {
            ITaskCommand taskCommand = context.Message;

            if (taskCommand == null)
            {
                _logger.LogInformation($"Error convert context.Message to {nameof(ITaskCommand)}");
                throw new NullReferenceException();
            }

            await _listenerExternalApi.ExecuteTestApi(taskCommand);

            _logger.LogInformation("Task command created.");
        }
    }
}