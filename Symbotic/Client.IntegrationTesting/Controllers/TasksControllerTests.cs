using Client.Infrastructure.Models;
using Client.IntegrationTesting.Common;
using Newtonsoft.Json;
using Share;
using Share.Models.Task;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Client.IntegrationTesting.Controllers
{
    public class TasksControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public TasksControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateTaskReturnStatus200()
        {
            IEnumerable<ApiEndPoint> endPoints = new List<ApiEndPoint>()
                                    { new ApiEndPoint() { EndpointUrl = "http://localhost:51830/" },
                                      new ApiEndPoint() { EndpointUrl = "http://localhost:51831/" }
                                    };

            var taskModel = new TaskModel()
            {
                EndPoints = endPoints,
                RequestQuantity = 10,
                Transport = Enums.TypeTransport.Http,
                Message = "Hello World"
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(taskModel), Encoding.UTF8, "application/json");
            var httpResponse = await _fixture.Client.PostAsync("/api/Tasks/Create", httpContent);

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}