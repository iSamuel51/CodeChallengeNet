using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels
{
    public class SalesDateRangeRequest
    {
        /// <summary>
        /// Gets or sets the Start Date of the desired date range.
        /// </summary>
        /// <value>The Start Date.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date of the desired date range.
        /// </summary>
        /// <value>The End Date.</value>
        public DateTime EndDate { get; set; }
    }
}