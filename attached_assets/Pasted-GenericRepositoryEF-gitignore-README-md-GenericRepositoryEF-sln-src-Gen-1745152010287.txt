GenericRepositoryEF/
├── .gitignore
├── README.md
├── GenericRepositoryEF.sln
│
├── src/
│   ├── GenericRepositoryEF.Core/
│   │   ├── GenericRepositoryEF.Core.csproj
│   │   │
│   │   ├── Interfaces/
│   │   │   ├── IEntity.cs
│   │   │   ├── IEntityWithKey.cs
│   │   │   ├── IRepository.cs
│   │   │   ├── IReadOnlyRepository.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   ├── ISpecification.cs
│   │   │   ├── ISoftDelete.cs
│   │   │   ├── IAuditableEntity.cs
│   │   │   └── ICachedRepository.cs
│   │   │
│   │   ├── Models/
│   │   │   ├── PagedResult.cs
│   │   │   └── OrderByDirection.cs
│   │   │
│   │   ├── Specifications/
│   │   │   ├── BaseSpecification.cs
│   │   │   └── SpecificationBuilder.cs
│   │   │
│   │   └── Exceptions/
│   │       ├── EntityNotFoundException.cs
│   │       ├── ConcurrencyException.cs
│   │       ├── RepositoryException.cs
│   │       └── TransactionException.cs
│   │
│   ├── GenericRepositoryEF.Infrastructure/
│   │   ├── GenericRepositoryEF.Infrastructure.csproj
│   │   │
│   │   ├── Repositories/
│   │   │   ├── Repository.cs
│   │   │   ├── ReadOnlyRepository.cs
│   │   │   ├── CachedRepository.cs
│   │   │   └── NullRepository.cs
│   │   │
│   │   ├── UnitOfWork/
│   │   │   └── UnitOfWork.cs
│   │   │
│   │   ├── Specifications/
│   │   │   └── SpecificationEvaluator.cs
│   │   │
│   │   ├── Factories/
│   │   │   └── RepositoryFactory.cs
│   │   │
│   │   ├── Interceptors/
│   │   │   ├── AuditSaveChangesInterceptor.cs
│   │   │   └── SoftDeleteSaveChangesInterceptor.cs
│   │   │
│   │   └── Extensions/
│   │       ├── QueryableExtensions.cs
│   │       └── PredicateBuilder.cs
│   │
│   └── GenericRepositoryEF.Extensions/
│       ├── GenericRepositoryEF.Extensions.csproj
│       │
│       ├── DependencyInjection/
│       │   ├── ServiceCollectionExtensions.cs
│       │   └── ServiceConfigurationOptions.cs
│       │
│       ├── Configuration/
│       │   └── DbContextOptionsBuilderExtensions.cs
│       │
│       └── Extensions/
│           └── DbContextExtensions.cs
│
├── tests/
│   ├── GenericRepositoryEF.UnitTests/
│   │   ├── GenericRepositoryEF.UnitTests.csproj
│   │   │
│   │   ├── Repositories/
│   │   │   ├── RepositoryTests.cs
│   │   │   ├── ReadOnlyRepositoryTests.cs
│   │   │   ├── CachedRepositoryTests.cs
│   │   │   └── NullRepositoryTests.cs
│   │   │
│   │   ├── Specifications/
│   │   │   ├── BaseSpecificationTests.cs
│   │   │   ├── SpecificationBuilderTests.cs
│   │   │   └── SpecificationEvaluatorTests.cs
│   │   │
│   │   ├── UnitOfWork/
│   │   │   └── UnitOfWorkTests.cs
│   │   │
│   │   └── Factories/
│   │       └── RepositoryFactoryTests.cs
│   │
│   └── GenericRepositoryEF.IntegrationTests/
│       ├── GenericRepositoryEF.IntegrationTests.csproj
│       │
│       ├── Setup/
│       │   ├── TestDbContext.cs
│       │   ├── TestEntity.cs
│       │   └── TestFixture.cs
│       │
│       ├── Fixtures/
│       │   ├── SqlServerFixture.cs
│       │   ├── PostgreSqlFixture.cs
│       │   ├── OracleFixture.cs
│       │   └── SqliteFixture.cs
│       │
│       ├── Repositories/
│       │   ├── RepositoryIntegrationTests.cs
│       │   └── ReadOnlyRepositoryIntegrationTests.cs
│       │
│       ├── UnitOfWork/
│       │   └── UnitOfWorkIntegrationTests.cs
│       │
│       └── Performance/
│           └── RepositoryPerformanceTests.cs
│
├── samples/
│   ├── SampleApi/
│   │   ├── SampleApi.csproj
│   │   │
│   │   ├── Controllers/
│   │   │   └── ProductsController.cs
│   │   │
│   │   ├── Data/
│   │   │   ├── AppDbContext.cs
│   │   │   └── AppDbContextFactory.cs
│   │   │
│   │   ├── Models/
│   │   │   ├── Product.cs
│   │   │   ├── Category.cs
│   │   │   └── Supplier.cs
│   │   │
│   │   ├── Specifications/
│   │   │   └── ProductSpecifications.cs
│   │   │
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── SampleConsole/
│   │   ├── SampleConsole.csproj
│   │   │
│   │   ├── Data/
│   │   │   └── ConsoleDbContext.cs
│   │   │
│   │   ├── Models/
│   │   │   └── Customer.cs
│   │   │
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   └── SampleBlazor/
│       ├── SampleBlazor.csproj
│       │
│       ├── Data/
│       │   └── BlazorDbContext.cs
│       │
│       ├── Models/
│       │   └── TodoItem.cs
│       │
│       ├── Pages/
│       │   └── TodoList.razor
│       │
│       ├── Services/
│       │   └── TodoService.cs
│       │
│       ├── Program.cs
│       └── appsettings.json
│
└── docs/
    ├── architecture.md
    ├── implementation-decisions.md
    ├── usage-examples.md
    └── diagrams/
        ├── repository-pattern.png
        ├── unit-of-work.png
        └── specification-pattern.png