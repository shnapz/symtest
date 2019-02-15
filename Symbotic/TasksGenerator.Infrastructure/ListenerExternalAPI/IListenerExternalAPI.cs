using Contracts.Tasks;
using System.Threading.Tasks;

namespace TasksGenerator.Infrastructure.ListenerExternal
{
    internal interface IListenerExternalApi
    {
        /// <summary>
        /// Sending a request to external API using randomly URL
        /// </summary>
        /// <param name="taskCommand">Task for processing  </param>
        /// <returns></returns>
        Task ExecuteTestApi(ITaskCommand taskCommand);
    }
}