using Contracts.Tasks;
using MassTransit;
using System;
using System.Threading.Tasks;
using TasksGenerator.Infrastructure.ListenerExternal;

namespace TasksGenerator.Infrastructure.ServiceBus
{
    internal sealed class TasksHandler : IConsumer<ITaskCommand>
    {
        private readonly IListenerExternalApi _listenerExternalApi;

        public TasksHandler(IListenerExternalApi listenerExternalApi)
        {
            _listenerExternalApi = listenerExternalApi;
        }

        public async Task Consume(ConsumeContext<ITaskCommand> context)
        {
            ITaskCommand taskCommand = context.Message;

            if (taskCommand == null)
            {
                //ToDo Log
                throw new NullReferenceException();
            }

            await _listenerExternalApi.ExecuteTestApi(taskCommand);

            await Console.Out.WriteLineAsync("ITaskCommand created.");
        }
    }
}