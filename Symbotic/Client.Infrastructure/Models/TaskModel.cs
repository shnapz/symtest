using Share.Models.Task;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Share.Enums;

namespace Client.Infrastructure.Models
{
    /// <summary>
    /// Model for settings task
    /// </summary>
    public sealed class TaskModel
    {
        /// <summary>
        /// Type transport
        /// </summary>
        [Required]
        public TypeTransport Transport { get; set; }

        /// <summary>
        /// Quantity requests to external Api
        /// </summary>
        public int RequestQuantity { get; set; }

        /// <summary>
        /// URLs  external API
        /// </summary>
        public IEnumerable<ApiEndPoint> EndPoints { get; set; }

        /// <summary>
        /// Message body to external API
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}