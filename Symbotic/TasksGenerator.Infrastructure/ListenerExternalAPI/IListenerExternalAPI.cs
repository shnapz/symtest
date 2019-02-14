using Contracts.Tasks;
using System.Threading.Tasks;

namespace TasksGenerator.Infrastructure.ListenerExternal
{
    internal interface IListenerExternalApi
    {
        Task ExecuteTestApi(ITaskCommand taskCommand);
    }
}