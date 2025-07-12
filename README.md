# MarketPOS.API
MarketPOS.API is a smart Point of Sale (POS) system built with .NET 9 and Clean Architecture. It manages products, categories, pricing, and discounts. Supports multi-units (carton, strip, pill), localization, unified responses, user roles, and uses CQRS, FluentValidation, and Localization.

#📒 Project Notes – MarketPOS.API

**MarketPOS.API Project Notes for Youssef**

---

### ✅ General Info:

* Built with **.NET 9**, **Clean Architecture**, and **CQRS**.
* API handles: Products, Categories, Prices, Discounts, Multi-units, Localization, User Permissions.

---

### ⚙ Project Layers:

* **API Layer:** Entry point, contains controllers, middlewares.
* **Application Layer:** Contains CQRS (Commands, Queries, Handlers), Validation, DTOs.
* **Infrastructure Layer:** EF Core, Repositories, Configuration.
* **Domain Layer:** Entities, Enums, Business logic.
* **Shared Layer:** Common utilities, ResultDto, base classes.
* **Design Layer:** Strategy patterns, design implementations.

---

### 🔨 Important Concepts:

* **CQRS:** Separate read (Query) and write (Command) logic.
* **IUnitOfWork:** Handles transactions and repository access.
* **GenericRepository:** Base repo with common EF operations.
* **Soft Delete:** Uses IsDeleted + query filtering.
* **FluentValidation:** Used for input validation.
* **Localization:** Implemented via custom `JsonStringLocalizer`.

---

### 🛋️ Memory Optimization Tips:

* Use `.AsNoTracking()` for read-only queries.
* Prefer `.ProjectTo<DTO>()` over `.Map()` + `ToList()` to reduce memory usage.
* Avoid heavy `Include()` unless necessary. Use `.Select()` or flatten data.

---

### 🌐 Localization Notes:

* Custom implementation using `JsonStringLocalizer`.
* Caching done via `ConcurrentDictionary<string, string>`.
* Cache is cleared on app shutdown using `ApplicationStopping`.

---

### 🌐 JSON Localization File:

* Location: `Resources/{CultureName}.json`
* Format: `{"Key": "Translation"}`

---

### ❌ Exception Handling:

* `ExceptionMiddleware`: Catches unhandled exceptions.
* `ResultDtoException`: Custom exception for business logic.
* `ExtendedProblemDetails`: Returns detailed errors to client.

---

### 🚀 Response Standardization:

* `ResponseMiddleware`: Wraps all output in `ApiResponse<T>`.
* Consistent structure: `Success`, `Message`, `Data`, `Errors`

---

### 📅 DTO Mapping:

* Use AutoMapper with `ProjectTo<T>()` for performance.
* Define profiles in `Design/Mappings/` or dedicated `Profiles/` folder.

---

### ⚡ Tips to Remember:

* Don’t include `.vs`, `bin`, or `obj` in Git (use `.gitignore`).
* Always pull before push to avoid conflicts.
* Use `ResultDto<T>` for standardized responses in Handlers.
* Watch your memory during queries, especially with `Include()`.

---

### ✏ To-Do Ideas:

* Add caching layer (e.g., MemoryCache or Redis).
* Improve Swagger documentation.
* Add unit tests using xUnit.
* Consider using Serilog for better logging.


