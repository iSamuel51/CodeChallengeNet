using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class CustomerViewModel
    {
        /// <summary>
        /// Gets or sets the Customer identificator
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the Customer name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}