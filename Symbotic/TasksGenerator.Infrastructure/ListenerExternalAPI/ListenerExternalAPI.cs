using Contracts.Tasks;
using MassTransit;
using Share.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TasksGenerator.Infrastructure.Providers;

namespace TasksGenerator.Infrastructure.ListenerExternal
{
    public sealed class ListenerExternalApi : IListenerExternalApi
    {
        private readonly ITestExternalApiProvider<HttpStatusCode> _testExternalApiProvider;
        private readonly IBusControl _serviceBus;
        private readonly  

        public ListenerExternalApi(ITestExternalApiProvider<HttpStatusCode> testExternalApiProvider, IBusControl serviceBus)
        {
            _testExternalApiProvider = testExternalApiProvider;
            _serviceBus = serviceBus;
        }

        public async Task ExecuteTestApi(ITaskCommand taskCommand)
        {
            ICollection<HttpStatusCode> statusCodeList = new List<HttpStatusCode>(taskCommand.RequestQuantity);

            var random = new Random();

            for (int i = 0; i < taskCommand.RequestQuantity; i++)
            {
                var apiEndPointUrl = GetRendomUrl(taskCommand.EndPoints, random);

                var statusCode = await _testExternalApiProvider.SendRequestExternalApiAsync(taskCommand.Message, apiEndPointUrl);

                statusCodeList.Add(statusCode);
            }

            await _serviceBus.Publish(new TaskExecutedEvent() { Statistic = GetStatistic(statusCodeList) });
        }

        private string GetRendomUrl(IEnumerable<ApiEndPoint> endPoints, Random random)
        {
            int indexEndPoints = random.Next(0, endPoints.Count());

            return endPoints.ToArray()[indexEndPoints].EndpointUrl;
        }

        private IEnumerable<TaskStatistic> GetStatistic(ICollection<HttpStatusCode> httpStatusCodes)
        {
            return (from httpStatusCode in httpStatusCodes
                    group httpStatusCode by httpStatusCode into statusCode
                    select new TaskStatistic() { StatusCode = statusCode.Key,
                                                 StatusCodesQuantity = statusCode.Count() }).AsEnumerable();
        }
    }
}