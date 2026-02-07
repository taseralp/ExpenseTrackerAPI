using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
    public class CreateExpenseDto
    {
        [Required]
        public string Title { get; set; }

        [Required]

        [StringLength(45)]
        public string Category { get; set; }

        [Required]
        [Range(0.01,double.MaxValue)]
        public decimal Price { get; set; }

    }
}
