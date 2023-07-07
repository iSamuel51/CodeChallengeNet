using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class ConceptViewModel
    {
        /// <summary>
        /// Gets or sets the Concept identificator
        /// </summary>
        public int ConceptId { get; set; }

        /// <summary>
        /// Gets or sets the Quantity
        /// </summary>
        [Required]
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the Product Identificator
        /// </summary>
        /// <value>The Product Identificator.</value>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Sale Identificator
        /// </summary>
        /// <value>The Sale Identificator.</value>
        [Required]
        public int SaleId { get; set; }

        /// <summary>
        /// Gets or sets the Unit Price
        /// </summary>
        [Required]
        public double UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the Amount
        /// </summary>
        [Required]
        public double Amount { get; set; }
    }
}