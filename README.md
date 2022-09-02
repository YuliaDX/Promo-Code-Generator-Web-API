# Promo-Code-Generator

This is a sample ASP.NET Core Web API "monolith" project. This application is implemented according to the hexagonal/onion architecture. The ASP.NET Core project uses REST API.

# Implementation Details

The application core layer is represented by the **Core** project. It contains entities located in the "Domain" folder and the IRepository<T> interface (abstractions for operations with entities) located in the "Repositories" folder.

The infrastructure layer is represented by the **DataAccess** project. It contains Entity Framework Core DbContext located in the "Data" folder and data access implementations (the EFRepository<T> repository) located in the "Repositories" folder.

The UI layer is represented by the **PromocodeFactoryProject** project. It contains controllers located in the "Controllers" folder, a custom filter located in the "ErrorHandling" folder, and model objects (DTOs) located in the "Model" folder. This project uses Swagger UI (the NSwag.AspNetCore library).

The project operates with an SQLite database.
