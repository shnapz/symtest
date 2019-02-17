using Contracts.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Share;
using TasksGenerator.Infrastructure.ListenerExternal;
using TasksGenerator.Infrastructure.Providers;
using Xunit;
using Share.Models.Task;

namespace TasksGenerator.Test
{


    public class ListenerExternalApiTest
    {
        readonly Mock<ITransportProvider<HttpStatusCode>> _httpTransportMock;
        readonly Mock<IBusControl> _serviceBusMock;
        readonly Mock<ILogger<ListenerExternalApi>> _loggerMock;


        public ListenerExternalApiTest()
        {
            _httpTransportMock = new Mock<ITransportProvider<HttpStatusCode>>();
            _serviceBusMock = new Mock<IBusControl>();
            _loggerMock = new Mock<ILogger<ListenerExternalApi>>();
        }


        [Fact]
        public void SendRequestExternalApiAsyncRunRequestQuantityTimes()
        {

            // Arrange
            IEnumerable<ApiEndPoint> endPoints = new List<ApiEndPoint>()
            { new ApiEndPoint() { EndpointUrl = "http://localhost:51830/" },
                new ApiEndPoint() { EndpointUrl = "http://localhost:51831/" }
            };

            var taskModel = new TaskCommand()
            {
                EndPoints = endPoints,
                RequestQuantity = 10,
                Transport = Enums.TypeTransport.Http,
                Message = "Hello World"
            };

            var messageExternalApi = new MessageExternalApi() { Message = taskModel.Message };

            //Act
            _httpTransportMock.Setup
                (x => x.SendRequestExternalApiAsync(It.IsAny<MessageExternalApi>(), It.IsAny<String>()))
                .ReturnsAsync(It.IsAny<HttpStatusCode>());

            var listenerExternalApi = new ListenerExternalApi(_httpTransportMock.Object, _serviceBusMock.Object, _loggerMock.Object);

            listenerExternalApi.ExecuteTestApi(taskModel).Wait();

            //Assert
            _httpTransportMock.Verify
                   (x => x.SendRequestExternalApiAsync(
                                                       It.IsAny<MessageExternalApi>(), It.IsAny<String>()),
                                                       Times.Exactly(taskModel.RequestQuantity)
                                                        );
        }



    }
}
