using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("Expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(ILogger<ExpenseController> logger)
        {
            _logger = logger;
        }

        public static List<Expense> Expenses = new List<Expense>
        {
            new Expense { Id = 1, Category = "erik", Date = DateTime.Now, Price = 2, Title = "Elma"}

        };

        //Gets All Items in The List
        [HttpGet]
        public IActionResult GetExpenses()
        {
            if (Expenses.Count() != 0)
            {
                _logger.LogInformation("All items were listed successfully.");
                return Ok(Expenses);
            }

            _logger.LogError("The list retrieval failed because no expenses were found.");
            return NotFound("No expenses found.");

        }

        //Gets Item Respect To Id
        [HttpGet("{ItemId}")]
        public IActionResult GetItemById(int ItemId)
        {
            var Item = Expenses.FirstOrDefault(a => a.Id == ItemId);

            if (Item == null)
            {
                _logger.LogError("Could not find any expense associated with the entered ID");
                return NotFound("No expense was found with the ID you entered.");
            }

            _logger.LogInformation("The title for the entered ID has been successfully listed.");
            return Ok(Item);
        }

        //Gets Some Statistics Of Expenses
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            // Liste boşsa hata vermesin diye kontrol (Average boş listede hata verebilir)
            if (!Expenses.Any())
            {
                return Ok(new { Message = "Veri yok kirve, istatistik hesaplanamadı." });
            }

            var priceSum = Expenses.Sum(x => x.Price);
            var averageExpenses = Expenses.Average(x => x.Price);
            var expenseCount = Expenses.Count();

            _logger.LogInformation("Statistics calculated successfully.");

            // İşte sihirli dokunuş: İsimsiz Obje (Anonymous Object)
            return Ok(new
            {
                TotalSpent = priceSum,
                AverageSpend = averageExpenses,
                TotalTransactionCount = expenseCount
            });
        }

        //gets item & items respect to spesific search 
        [HttpGet("Query")]
        public IActionResult GetExpenseQuery
        (
            [FromQuery] int? Id,
            [FromQuery] string? Category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? StartDate
        )
        {
            var query = Expenses.AsQueryable();


            if (Id.HasValue)
            {
                _logger.LogInformation("Id Has Value");
                query = query.Where(a => a.Id == Id);
            }


            if (!string.IsNullOrEmpty(Category))
            {
                _logger.LogInformation("Category isnt Empty");
                query = query.Where(a => a.Category.ToLower().Contains(Category.ToLower()));
            }


            if (minPrice.HasValue && minPrice >= 0)
            {
                _logger.LogInformation("Minimum Price Has Value And Bigger Zero");
                query = query.Where(a => a.Price >= minPrice);
            }


            if (maxPrice.HasValue && maxPrice >= 0)
            {
                _logger.LogInformation("Maximum Price Has Value And Bigger Zero");
                query = query.Where(a => a.Price <= maxPrice);
            }

            if (minPrice < 0 || maxPrice < 0)
            {
                _logger.LogError("No valid prices were entered for the minimum and maximum fields");
                return BadRequest("Please Enter Positive Number");
            }


            if (StartDate.HasValue)
            {
                _logger.LogInformation("Start date has a value");
                query = query.Where(a => a.Date > StartDate);
            }

            if (query.Count() == 0)
            {
                _logger.LogError("No items were found that match the specified criteria");
                return NotFound("The requested expense was not found in the list.");
            }

            _logger.LogInformation("Items matching the criteria have been successfully listed.");
            return Ok(query);
        }

        //Add Expense in List
        [HttpPost]
        public IActionResult AddExpense([FromBody] CreateExpenseDto createExpense)
        {
            int newid;

            if (Expenses.Count == 0)
            {
                newid = 1;
            }
            else
            {
                newid = Expenses.Max(x => x.Id) + 1;
            }

            if (createExpense.Price < 0)
            {
                _logger.LogError("The value entered for the 'price' variable is not a valid price");
                return BadRequest("The value entered for the 'price' variable is not a valid price");
            }

            if (!string.IsNullOrEmpty(createExpense.Category) && !string.IsNullOrEmpty(createExpense.Title))
            {

                var createexpense = new Expense
                {
                    Id = newid,
                    Category = createExpense.Category,
                    Title = createExpense.Title,
                    Price = createExpense.Price,
                    Date = DateTime.Now
                };

                Expenses.Add(createexpense);
                _logger.LogInformation("Expense successfully created and appended to the list.");
                return Ok(Expenses);

            }
            else
            {
                _logger.LogError("Category or title fields cannot be empty.");
                return BadRequest("Category or Title cannot be empty.");
            }
        }

        //Finds Id and Delete The Item on List
        [HttpDelete("{ItemId}")]
        public IActionResult DeleteItem(int ItemId)
        {
            var item = Expenses.FirstOrDefault(a => a.Id == ItemId);

            if (item == null)
            {
                _logger.LogError("Product not found for this ID.");
                return NotFound();
            }

            Expenses.Remove(item);

            _logger.LogInformation("Item successfully deleted for the given ID.");
            return Ok(Expenses);
        }

        [HttpPut("{ItemId}")]
        public IActionResult UpdateAllInfo([FromBody] UpdateAllInfoDto updateAll, int ItemId) 
        {
            var updateitem = Expenses.FirstOrDefault(a => a.Id == ItemId);

            if (updateitem == null)
            {
                _logger.LogError("Product not found for this ID.");
                return NotFound();
            }

            updateitem.Title = updateAll.Title;

            if (updateAll.Price.HasValue) { updateitem.Price = updateAll.Price.Value; }

            updateitem.Category = updateAll.Category;

            _logger.LogInformation("Product successfully updated.");
            return Ok(updateitem);
        }

        [HttpPatch("{ItemId}")]
        public IActionResult UpdateSpesific(int ItemId, UpdateSpesificInfo update) 
        {

            var updateitem = Expenses.FirstOrDefault(x => x.Id == ItemId);
            if (updateitem == null)
            { 
                _logger.LogError("Product not found for this ID.");
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(update.Title) && update.Title != "string")
                updateitem.Title = update.Title;

            if (!string.IsNullOrWhiteSpace(update.Category) && update.Category != "string")
                updateitem.Category = update.Category;

            if (update.Price.HasValue)
                updateitem.Price = update.Price.Value;

            _logger.LogInformation("Product successfully updated.");
            return Ok(updateitem);
        }

    }
}
