﻿using Contracts.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Client.Infrastructure.ServiceBus
{
    /// <summary>
    /// Handling TaskExecutedEvent. Statistics output
    /// </summary>
    public sealed class StatisticHandler : IConsumer<ITaskExecutedEvent>
    {
        private readonly ILogger _logger;

        public StatisticHandler(ILogger<StatisticHandler> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ITaskExecutedEvent> context)
        {
            ITaskExecutedEvent taskExecutedEvent = context.Message;

            if (taskExecutedEvent == null)
            {
                var logMessage  = $"{nameof(taskExecutedEvent)} mustn't be null .";

                _logger.LogInformation(logMessage);
                throw new NullReferenceException(logMessage);
            }

            _logger.LogInformation("Calls statistics:");

            foreach (var statistic in taskExecutedEvent.Statistic)
            {
                _logger.LogInformation($" StatusCode: {statistic.StatusCode} ; StatusCodesQuantity: {statistic.StatusCodesQuantity} ");
            }

            return Task.FromResult(0);
        }
    }
}