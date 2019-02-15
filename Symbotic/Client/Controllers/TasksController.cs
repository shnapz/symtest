using Client.Infrastructure;
using Client.Infrastructure.Models;
using Contracts.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly IBusControl _busControl;
        private readonly AppSettings _appSettings;

        public TasksController(IBusControl busControl, IOptions<AppSettings> appSettings)
        {
            _busControl = busControl;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel taskModel)
        {
            if (taskModel == null)
            {
                return BadRequest("Incorrect data.");
            }

            if (!taskModel.EndPoints.Any())
            {
                return BadRequest("Haven't set any EndPoints.");
            }

            if (taskModel.RequestQuantity == 0)
            {
                return BadRequest("Request quantity should be more than 0");
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var sendEndpoint = await _busControl.GetSendEndpoint(
                                  new Uri($"{_appSettings.ServiceBusConnection.Host}{Contracts.ServiceBusQueues.RequestGenerator}"));

            await sendEndpoint.Send(new TaskCommand
            {
                RequestQuantity = taskModel.RequestQuantity,
                Transport = taskModel.Transport,
                EndPoints = taskModel.EndPoints,
                Message = taskModel.Message
            });

            return Ok("Task has created successfully.");
        }
    }
}