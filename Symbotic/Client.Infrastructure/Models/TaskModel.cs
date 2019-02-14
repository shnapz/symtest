using Share.Models.Task;
using System.Collections.Generic;
using static Share.Enums;

namespace Client.Infrastructure.Models
{
    public sealed class TaskModel
    {
        public TypeTransport Transport { get; set; }

        public int RequestQuantity { get; set; }

        public IEnumerable<ApiEndPoint> EndPoints { get; set; }

        public string Message { get; }
    }
}