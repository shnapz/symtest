using System.Collections.Generic;

namespace Contracts.Tasks
{
    public interface ITaskExecutedEvent
    {
        IEnumerable<TaskStatistic> Statistic { get; }
    }
}