using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;

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

        //Gets All Items in The List
        [HttpGet]
        public IActionResult GetExpenses(int page = 1,int pageSize = 10)
        {
            if (ApplicationContext.Expenses.Count() != 0)
            {
                var expansesactive = ApplicationContext.Expenses
                    .Where(x => x.isDeleted == false)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation("All items were listed successfully.");
                return Ok(expansesactive);
            }

            _logger.LogError("The list retrieval failed because no expenses were found.");
            return NotFound("No expenses found.");

        }

        //Gets Item Respect To Id
        [HttpGet("{ItemId}")]
        public IActionResult GetItemById(int ItemId)
        {
            var Item = ApplicationContext.Expenses.FirstOrDefault(a => a.Id == ItemId && a.isDeleted == false);

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
            if (!ApplicationContext.Expenses.Any())
            {
                return Ok(new { Message = "Veri yok kirve, istatistik hesaplanamadı." });
            }

            var activespend = ApplicationContext.Expenses.Where(_ => _.isDeleted == false);

            var priceSum = activespend.Sum(x => x.Price);
            
            var averageExpenses = activespend.Average(x => x.Price);
            
            var expenseCount = activespend.Count();
            
            var mostexpensive = activespend
                .OrderByDescending(a => a.Price)
                .FirstOrDefault();

            var topCategory = activespend
            .GroupBy(x => x.Category)
            .Select(g => new
            {
                Category = g.Key,
                Total = g.Sum(x => x.Price)
            })
            .OrderByDescending(x => x.Total)
            .FirstOrDefault();

            _logger.LogInformation("Statistics calculated successfully.");

            return Ok(new
            {
                TotalSpent = priceSum,
                AverageSpend = averageExpenses,
                TotalTransactionCount = expenseCount,
                mostspend = mostexpensive,
                mostspendcategory = topCategory
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
            var query = ApplicationContext.Expenses.AsQueryable();


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

            query = query.Where(a => a.isDeleted == false);

            _logger.LogInformation("Items matching the criteria have been successfully listed.");
            return Ok(query);
        }

        //Add Expense in List
        [HttpPost]
        public IActionResult AddExpense([FromBody] CreateExpenseDto createExpense)
        {
            int newid;

            if (ApplicationContext.Expenses.Count == 0)
            {
                newid = 1;
            }
            else
            {
                newid = ApplicationContext.Expenses.Max(x => x.Id) + 1;
            }
                      
                var createexpense = new Expense
                {
                    Id = newid,
                    Category = createExpense.Category,
                    Title = createExpense.Title,
                    Price = createExpense.Price,
                    Date = DateTime.Now,
                    isDeleted = false,
                    
                };

                ApplicationContext.Expenses.Add(createexpense);
                _logger.LogInformation("Expense successfully created and appended to the list.");
                return Created();
                       
           
        }

        //Finds Id and changes isDeleted True
        [HttpDelete("{ItemId}")]
        public IActionResult DeleteItem(int ItemId)
        {
            var item = ApplicationContext.Expenses.FirstOrDefault(a => a.Id == ItemId);

            if (item == null)
            {
                _logger.LogError("Product not found for this ID.");
                return NotFound();
            }

            item.isDeleted = true;

            _logger.LogInformation("Item successfully deleted for the given ID.");
            return NoContent();
        }

        [HttpPut("{ItemId}")]
        public IActionResult UpdateAllInfo([FromBody] UpdateAllInfoDto updateAll, int ItemId) 
        {
            var updateitem = ApplicationContext.Expenses.FirstOrDefault(a => a.Id == ItemId);

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

            var updateitem = ApplicationContext.Expenses.FirstOrDefault(x => x.Id == ItemId);
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

        //Gets Deleted expenses
        [HttpGet("Deleted")]
        public IActionResult GetDeleted() 
        {
            var deletedexpanses = ApplicationContext.Expenses.Where(a => a.isDeleted == true).ToList();

            if (deletedexpanses.Count() != 0) 
            {
                _logger.LogInformation("Deleted Expanses Showed Successfully");
                return Ok(deletedexpanses);
            }
            _logger.LogInformation("Couldn't found any deleted expanses");
            return NotFound();
        }
    }
}
