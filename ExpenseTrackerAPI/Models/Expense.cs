using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [MinLength(1)]
        public string Title { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(45)]
        [MinLength(1)]
        public string Category { get; set; }

        public DateTime Date { get; set; }

        public bool isDeleted { get; set; } 
    }
}
