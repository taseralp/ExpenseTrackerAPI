namespace ExpenseTrackerAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }
    }
}
