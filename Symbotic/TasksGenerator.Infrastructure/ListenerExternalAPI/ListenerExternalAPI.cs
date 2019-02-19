using Contracts.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Share.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TasksGenerator.Infrastructure.Providers;

namespace TasksGenerator.Infrastructure.ListenerExternal
{
    /// <summary>
    /// Sending a requests to external Api.
    /// </summary>
    public sealed class ListenerExternalApi : IListenerExternalApi
    {
        private readonly ITransportProvider<HttpStatusCode> _httpTransport;
        private readonly IBusControl _serviceBus;
        private readonly ILogger _logger;

        public ListenerExternalApi(ITransportProvider<HttpStatusCode> testExternalApiProvider,
                                   IBusControl serviceBus,
                                   ILogger<ListenerExternalApi> logger)
        {
            _httpTransport = testExternalApiProvider;
            _serviceBus = serviceBus;
            _logger = logger;
        }

        public async Task ExecuteTestApi(ITaskCommand taskCommand)
        {
            await _serviceBus.Publish(new StartExecutingTestEvent());

            ICollection<HttpStatusCode> statusCodeList = new List<HttpStatusCode>(taskCommand.RequestQuantity);

            var random = new Random();
            HttpStatusCode statusCode = 0;
            string apiEndPointUrl;

            var messageExternalApi = new MessageExternalApi() { Message = taskCommand.Message };

            for (int i = 0; i < taskCommand.RequestQuantity; i++)
            {
                apiEndPointUrl = GetRendomUrl(taskCommand.EndPoints, random);

                try
                {
                    statusCode = await _httpTransport.SendRequestExternalApiAsync(messageExternalApi, apiEndPointUrl);
                    statusCodeList.Add(statusCode);
                }
                catch (Exception ex)
                {
                    statusCodeList.Add(HttpStatusCode.InternalServerError);
                    _logger.LogInformation(ex, $"Error request external Api:{apiEndPointUrl}");
                }
            }

            //The event about executing all requests. Passing statistic of requests.
            await _serviceBus.Publish(new TaskExecutedEvent() { Statistic = GetStatistic(statusCodeList) });
        }

        /// <summary>
        /// Getting Url endpoints randomly.
        /// </summary>
        /// <param name="endPoints">Url endpoints</param>
        /// <param name="random">Random</param>
        /// <returns></returns>
        private string GetRendomUrl(IEnumerable<ApiEndPoint> endPoints, Random random)
        {
            int indexEndPoints = random.Next(0, endPoints.Count());

            return endPoints.ToArray()[indexEndPoints].EndpointUrl;
        }

        /// <summary>
        /// Creating requests statistics
        /// </summary>
        /// <param name="httpStatusCodes">Status codes</param>
        /// <returns>TaskStatistic</returns>
        private IEnumerable<TaskStatistic> GetStatistic(ICollection<HttpStatusCode> httpStatusCodes)
        {
            return (from httpStatusCode in httpStatusCodes
                    group httpStatusCode by httpStatusCode into statusCode
                    select new TaskStatistic()
                    {
                        StatusCode = statusCode.Key,
                        StatusCodesQuantity = statusCode.Count()
                    }).AsEnumerable();
        }
    }
}