using Contracts.Tasks;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Client.Infrastructure.ServiceBus
{
    public sealed class StatisticHandler : IConsumer<ITaskExecutedEvent>
    {
        public async Task Consume(ConsumeContext<ITaskExecutedEvent> context)
        {
            ITaskExecutedEvent taskExecutedEvent = context.Message;

            if (taskExecutedEvent == null)
            {
                throw new NullReferenceException(); //ToDo Log
            }

            await Console.Out.WriteLineAsync("Calls statistics.");

            foreach (var statistic in taskExecutedEvent.Statistic)
            {
                await Console.Out.WriteLineAsync($" StatusCode: {statistic.StatusCode} ; StatusCodesQuantity: {statistic.StatusCodesQuantity} ");
            }
        }
    }
}