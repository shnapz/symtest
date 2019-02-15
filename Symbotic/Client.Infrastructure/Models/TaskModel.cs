using Share.Models.Task;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Share.Enums;

namespace Client.Infrastructure.Models
{
    public sealed class TaskModel
    {
        [Required]
        public TypeTransport Transport { get; set; }

        public int RequestQuantity { get; set; }

        public IEnumerable<ApiEndPoint> EndPoints { get; set; }

        [Required]
        public string Message { get; set; }
    }
}