# Promo-Code-Generator

This is a sample ASP.NET Core Web API "monolith" project. This application is implemented according to the hexagonal/onion architecture. The ASP.NET Core project uses REST API.

# Implementation Details

The application core layer is represented by the **Core** project. It contains entities located in the "Domain" folder and the IRepository<T> interface (abstractions for operations with entities) located in the "Repositories" folder.

The infrastructure/Data Access layer is represented by the **DataAccess** project. It contains Entity Framework Core DbContext located in the "Data" folder and data access implementations (the EFRepository<T> repository) located in the "Repositories" folder. The EFRepository<T> class is implemented according to the Repository pattern.

The Business Logic layer is represented by the **BusinessLogic** project. It contains abstractions for services (interfaces located in the "Abstractions" folder) and services that include business logic to work with entities (the "Services" folder).

The UI layer is represented by the **PromocodeFactoryProject** project. It contains controllers located in the "Controllers" folder, a custom filter located in the "ErrorHandling" folder, a custom middleware located in the "Middlewares" folder, and model objects (DTOs) located in the "Model" folder. This project uses Swagger UI (the NSwag.AspNetCore library).

The project operates with an SQLite database.

There are also sample unit tests implemented for the Partners controller and Partner service. The unit tests are located in the "UnitTests/WebHost/Controllers" and "UnitTests/BusinessLogic/Services" folders. The unit tests are implemented with the help of the xUnit library.
