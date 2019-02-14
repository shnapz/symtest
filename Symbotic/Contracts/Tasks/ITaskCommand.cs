using Share.Models.Task;
using System.Collections.Generic;
using static Share.Enums;

namespace Contracts.Tasks
{
    public interface ITaskCommand
    {
        TypeTransport Transport { get; }

        int RequestQuantity { get; }

        string Message { get; }

        IEnumerable<ApiEndPoint> EndPoints { get; }
    }
}