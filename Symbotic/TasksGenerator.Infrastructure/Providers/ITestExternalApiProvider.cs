using System.Threading.Tasks;

namespace TasksGenerator.Infrastructure.Providers
{
    public interface ITestExternalApiProvider<T>
    {
        Task<T> SendRequestExternalApiAsync(string messageBody, string endPointUrl);
    }
}