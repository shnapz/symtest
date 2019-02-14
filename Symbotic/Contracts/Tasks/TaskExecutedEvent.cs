using System.Collections.Generic;

namespace Contracts.Tasks
{
    public sealed class TaskExecutedEvent : ITaskExecutedEvent
    {
        public IEnumerable<TaskStatistic> Statistic { get; set; }
    }
}