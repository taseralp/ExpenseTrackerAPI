using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
    public class UpdateAllInfoDto
    {
        public string? Title { get; set; }

        [StringLength(45)]
        public string? Category { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }
    }
}
