using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class SaleViewModel
    {
        /// <summary>
        /// Gets or sets the Sale identificator
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// Gets or sets the Sale Date
        /// </summary>        
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Customer Identificator
        /// </summary>
        /// <value>The Customer Identificator.</value>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the Total
        /// </summary>
        [Required]
        public double Total { get; set; }

    }
}