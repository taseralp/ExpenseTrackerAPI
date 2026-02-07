using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Data
{
    public static class ApplicationContext
    {
        public static List<Expense> Expenses {  get; set; }

        static ApplicationContext()
        {

            Expenses = new List<Expense>()
            {
                new Expense(){ Id = 1, Category = "Car", Date = DateTime.Now.AddDays(-2), Price = 8855, Title = "Car Service", isDeleted = false},
                new Expense(){ Id = 2, Category = "Food", Date = DateTime.Now.AddDays(-5), Price = 420, Title = "Burger King", isDeleted = false},
                new Expense(){ Id = 3, Category = "Tech", Date = DateTime.Now.AddDays(-10), Price = 12500, Title = "Kulaklık", isDeleted = true},
                new Expense(){ Id = 4, Category = "Transport", Date = DateTime.Now.AddDays(-1), Price = 300, Title = "Taksi", isDeleted = false},
                new Expense(){ Id = 5, Category = "Bills", Date = DateTime.Now.AddDays(-15), Price = 980, Title = "Elektrik Faturası", isDeleted = false},
                new Expense(){ Id = 6, Category = "Food", Date = DateTime.Now.AddDays(-7), Price = 250, Title = "Döner", isDeleted = true},
                new Expense(){ Id = 7, Category = "Entertainment", Date = DateTime.Now.AddDays(-20), Price = 750, Title = "Sinema + Mısır", isDeleted = false},
                new Expense(){ Id = 8, Category = "Clothing", Date = DateTime.Now.AddDays(-30), Price = 3200, Title = "Mont", isDeleted = false},
                new Expense(){ Id = 9, Category = "Health", Date = DateTime.Now.AddDays(-3), Price = 600, Title = "Eczane", isDeleted = true},
                new Expense(){ Id = 10, Category = "Tech", Date = DateTime.Now.AddDays(-12), Price = 1800, Title = "Powerbank", isDeleted = false},
                new Expense(){ Id = 11, Category = "Car", Date = DateTime.Now.AddDays(-25), Price = 2100, Title = "Lastik Değişimi", isDeleted = false},
                new Expense(){ Id = 12, Category = "Food", Date = DateTime.Now.AddDays(-4), Price = 180, Title = "Kahve", isDeleted = true},
                new Expense(){ Id = 13, Category = "Bills", Date = DateTime.Now.AddDays(-18), Price = 450, Title = "İnternet Faturası", isDeleted = false},
                new Expense(){ Id = 14, Category = "Education", Date = DateTime.Now.AddDays(-40), Price = 950, Title = "Udemy Kursu", isDeleted = false},
                new Expense(){ Id = 15, Category = "Entertainment", Date = DateTime.Now.AddDays(-9), Price = 400, Title = "Oyun İçi Satın Alma", isDeleted = true}
            };


        }
    }
}
