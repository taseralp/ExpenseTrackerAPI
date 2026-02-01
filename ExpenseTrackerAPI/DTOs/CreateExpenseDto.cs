namespace ExpenseTrackerAPI.DTOs
{
    public class CreateExpenseDto
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

    }
}
