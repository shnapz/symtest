using Share.Models.Task;
using System.Collections.Generic;
using static Share.Enums;

namespace Contracts.Tasks
{
    public sealed class TaskCommand : ITaskCommand
    {
        public TypeTransport Transport { get; set; }

        public int RequestQuantity { get; set; }

        public IEnumerable<ApiEndPoint> EndPoints { get; set; }

        public string Message { get; set; }
    }
}