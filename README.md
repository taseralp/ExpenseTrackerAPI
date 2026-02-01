# 💰 Expense Tracker API

A robust RESTful Web API built with **.NET 8.0 (ASP.NET Core)** to manage personal expenses. This project demonstrates backend engineering best practices including **DTO patterns**, **LINQ queries**, **advanced filtering**, and **structured logging**.

![.NET 8.0](https://img.shields.io/badge/.NET%208.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)

## 🚀 Key Features

- **Advanced Querying:** Filter expenses by Category, Price Range (Min/Max), and Date using dynamic **LINQ** queries.
- **Statistics Endpoint:** Returns consolidated financial data (Total Spending, Average, Count) in a clean JSON format.
- **Data Transfer Objects (DTOs):** Separation of domain models and API contracts for security and maintainability.
- **Dependency Injection:** Integrated `ILogger` for tracking operations and debugging.
- **PATCH Support:** Partial updates for specific fields without overwriting the entire resource.
- **Robust Error Handling:** Proper HTTP status codes (200, 400, 404) and logical validations.

## 🛠️ Tech Stack

- **Framework:** .NET 8.0 (LTS)
- **Language:** C# 12
- **Architecture:** Controller-Service Pattern (Simplified)
- **Documentation:** Swagger UI

## 🔌 API Endpoints

### 🟢 GET Operations
- `GET /Expense` → Returns all expenses.
- `GET /Expense/{id}` → Returns a specific expense by ID.
- `GET /Expense/stats` → **(New)** Returns financial statistics (Total, Average, Count).
- `GET /Expense/Query?Category=Food&minPrice=10` → Filters expenses based on criteria.

### 🟡 POST & PUT/PATCH Operations
- `POST /Expense` → Adds a new expense (validated via DTO).
- `PUT /Expense/{id}` → Updates the entire expense record.
- `PATCH /Expense/{id}` → Updates specific fields (Title, Price, or Category).

### 🔴 DELETE Operations
- `DELETE /Expense/{id}` → Removes an expense from the list.

## 📦 Getting Started

1. **Clone the repo:**
   ```bash
   git clone [https://github.com/taseralp/ExpenseTrackerAPI.git](https://github.com/taseralp/ExpenseTrackerAPI.git)
