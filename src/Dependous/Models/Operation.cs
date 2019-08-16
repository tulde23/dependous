using System;

namespace Dependous.Models
{
    /// <summary>
    ///  Models an operation that can be timed.
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }
    }
}