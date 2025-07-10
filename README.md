# MarketPOS.API
MarketPOS.API is a smart Point of Sale (POS) system built with .NET 9 and Clean Architecture. It manages products, categories, pricing, and discounts. Supports multi-units (carton, strip, pill), localization, unified responses, user roles, and uses CQRS, FluentValidation, and Localization.

#📒 Project Notes – MarketPOS.API

##🔧 Architecture & Patterns
The project follows Clean Architecture with clear separation between layers:

Domain, Application, Infrastructure, API, Shared, and Design

Uses CQRS pattern with MediatR (each Command and Query in a separate file).

Implements Strategy Pattern for supplier and customer discount logic.

##🧱 Code Structure Guidelines
All responses follow a unified format using ResultDto.

Exceptions are handled through a custom ExceptionMiddleware, using ProblemDetails for Swagger documentation.

FluentValidation is integrated into all request models with full localization support.

AutoMapper is used to map entities to DTOs and vice versa.

##🌐 Localization
Multi-language support via IStringLocalizer.

Uses a custom ILocalizationPostProcessor to post-process localized values.

JSON-based translation files are stored in a dedicated localization folder.

##📦 Product Logic Notes
Supports multi-unit products (e.g., carton, strip, pill).

Medium and small unit prices are calculated automatically from the sale price.

Discounts can be defined either by percentage or by directly setting purchase price.

Products include fields such as expiration date, purchase/sale prices, and inventory count.

##🧪 Testing & Swagger
Full API documentation is available via Swagger UI.

Test all endpoints to ensure:

Valid and consistent responses.

Correct translation and localization messages.

Unit conversions and business logic behave as expected.

##📌 General Notes
Always follow SOLID principles when adding or refactoring code.

All Handlers should use IResultFactory<T> and IServiceFactory for consistency.

Middleware is responsible for shaping responses and applying the correct language.

Keep the product system flexible to support both supermarket and pharmacy use cases.

