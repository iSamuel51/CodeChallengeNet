using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class ProductViewModel
    {
        /// <summary>
        /// Gets or sets the Product identificator
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Product name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Unit Price
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        [Required]
        public double Cost { get; set; }
    }
}