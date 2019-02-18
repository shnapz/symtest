using Contracts.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Share;
using Share.Models.Task;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TasksGenerator.Infrastructure.ListenerExternal;
using TasksGenerator.Infrastructure.Providers;
using Xunit;

namespace TasksGenerator.Test
{
    public class ListenerExternalApiTest
    {
        private readonly Mock<ITransportProvider<HttpStatusCode>> _httpTransportMock;
        private readonly Mock<IBusControl> _serviceBusMock;
        private readonly Mock<ILogger<ListenerExternalApi>> _loggerMock;

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

            //Act
            _httpTransportMock.Setup
                (x => x.SendRequestExternalApiAsync(It.IsAny<MessageExternalApi>(), It.IsAny<String>()));
                //.ReturnsAsync(It.IsAny<HttpStatusCode>());

            var listenerExternalApi = new ListenerExternalApi(_httpTransportMock.Object, _serviceBusMock.Object, _loggerMock.Object);

            listenerExternalApi.ExecuteTestApi(taskModel).Wait();

            //Assert
            _httpTransportMock.Verify
                   (x => x.SendRequestExternalApiAsync(
                                                       It.IsAny<MessageExternalApi>(), It.IsAny<String>()),
                                                       Times.Exactly(taskModel.RequestQuantity)
                                                        );
        }

        [Fact]
        public void StartExecutingTestEventWasPublishing()
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

            //Act
            var listenerExternalApi = new ListenerExternalApi(_httpTransportMock.Object, _serviceBusMock.Object, _loggerMock.Object);

            listenerExternalApi.ExecuteTestApi(taskModel).Wait();

            //Assert
            _serviceBusMock.Verify(b => b.Publish(It.IsAny<StartExecutingTestEvent>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public void TaskExecutedEventWasPublishing()
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

            //Act
            var listenerExternalApi = new ListenerExternalApi(_httpTransportMock.Object, _serviceBusMock.Object, _loggerMock.Object);

            listenerExternalApi.ExecuteTestApi(taskModel).Wait();

            //Assert
            _serviceBusMock.Verify(b => b.Publish(It.IsAny<TaskExecutedEvent>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }



        [Fact]
        public void SendRequestExternalApiAsyncReturnStatusCode()
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

            //Act
            _httpTransportMock.Setup
                (x => x.SendRequestExternalApiAsync(It.IsAny<MessageExternalApi>(), It.IsAny<String>()))
                  .ReturnsAsync(It.IsAny<HttpStatusCode>());

            var listenerExternalApi = new ListenerExternalApi(_httpTransportMock.Object, _serviceBusMock.Object, _loggerMock.Object);

            listenerExternalApi.ExecuteTestApi(taskModel).Wait();

            //Assert
           _httpTransportMock.Verify(x => x.SendRequestExternalApiAsync(It.IsAny<MessageExternalApi>(), It.IsAny<String>()));
        }

    }
}