using System.Net;

namespace Contracts.Tasks
{
    public sealed class TaskStatistic
    {
        public HttpStatusCode StatusCode { get; set; }

        public int StatusCodesQuantity { get; set; }
    }
}