using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using GenericRepositoryEF.Extensions.DependencyInjection;
using GenericRepositoryEF.Sample.Console.Data;
using GenericRepositoryEF.Sample.Console.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Set up dependency injection
var services = new ServiceCollection();

// Add logging
services.AddLogging(configure => configure.AddConsole());

// Add memory cache for caching repositories
services.AddMemoryCache();

// Add sample application services
services.AddTransient<ICurrentUserService, ConsoleCurrentUserService>();
services.AddSingleton<SampleService>();

// Add DbContext with SQLite
services.AddDbContext<SampleDbContext>(options =>
{
    options.UseSqlite("Data Source=sample.db");
});

// Add Generic Repository with configuration
services.AddGenericRepository<SampleDbContext>(options =>
{
    options.AddAuditInterceptor = true;
    options.AddSoftDeleteInterceptor = true;
    options.AddDateTimeService = true;
});

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Ensure database is created and seeded
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
    SeedData(dbContext);
}

// Run the sample service
var sampleService = serviceProvider.GetRequiredService<SampleService>();
await sampleService.RunAsync();

// Helper method to seed data
static void SeedData(SampleDbContext dbContext)
{
    if (!dbContext.Products.Any())
    {
        var categories = new List<Category>
        {
            new Category { Name = "Electronics" },
            new Category { Name = "Books" },
            new Category { Name = "Clothing" }
        };

        dbContext.Categories.AddRange(categories);
        dbContext.SaveChanges();

        var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 999.99m, CategoryId = categories[0].Id, Description = "Powerful laptop with high-performance specs" },
            new Product { Name = "Smartphone", Price = 699.99m, CategoryId = categories[0].Id, Description = "Latest smartphone with advanced features" },
            new Product { Name = "Headphones", Price = 149.99m, CategoryId = categories[0].Id, Description = "Noise-canceling wireless headphones" },
            new Product { Name = "Programming Book", Price = 49.99m, CategoryId = categories[1].Id, Description = "Comprehensive guide to modern programming" },
            new Product { Name = "Novel", Price = 19.99m, CategoryId = categories[1].Id, Description = "Bestselling fiction novel" },
            new Product { Name = "T-Shirt", Price = 24.99m, CategoryId = categories[2].Id, Description = "Comfortable cotton t-shirt" },
            new Product { Name = "Jeans", Price = 59.99m, CategoryId = categories[2].Id, Description = "Classic denim jeans" }
        };

        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();
    }
}

// Implementation of ICurrentUserService for console application
public class ConsoleCurrentUserService : ICurrentUserService
{
    public string? UserId => "Console_User";
    public bool IsAuthenticated => true;
    public string? UserName => "Console User";
    public IEnumerable<string> Roles => new[] { "Admin" };
}

// Sample service to demonstrate the use of GenericRepositoryEF
public class SampleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SampleService> _logger;

    public SampleService(IUnitOfWork unitOfWork, ILogger<SampleService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Starting Sample Service");

        // 1. Basic repository operations
        await BasicRepositoryOperationsAsync();

        // 2. Specification pattern
        await SpecificationPatternAsync();

        // 3. Using cached repository
        await CachedRepositoryAsync();

        // 4. Using transaction
        await TransactionExampleAsync();

        _logger.LogInformation("Sample Service completed");
    }

    private async Task BasicRepositoryOperationsAsync()
    {
        _logger.LogInformation("---------- Basic Repository Operations ----------");

        // Get repository
        var productRepo = _unitOfWork.Repository<Product>();
        
        // Get all products
        var allProducts = await productRepo.GetAllAsync();
        _logger.LogInformation($"Found {allProducts.Count} products");
        
        // Get product by ID
        var firstProduct = allProducts.FirstOrDefault();
        if (firstProduct != null)
        {
            var foundProduct = await productRepo.FindAsync(firstProduct.Id);
            _logger.LogInformation($"Found product by ID: {foundProduct?.Name}");
        }
        
        // Add new product
        var newProduct = new Product
        {
            Name = "New Test Product",
            Price = 99.99m,
            Description = "A test product added through the repository",
            CategoryId = allProducts.First().CategoryId
        };
        
        var addedProduct = await productRepo.AddAsync(newProduct);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Added new product with ID: {addedProduct.Id}");
        
        // Update product
        addedProduct.Name = "Updated Test Product";
        productRepo.Update(addedProduct);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Updated product name to: {addedProduct.Name}");
        
        // Delete product
        productRepo.Delete(addedProduct);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Deleted product with ID: {addedProduct.Id}");
    }

    private async Task SpecificationPatternAsync()
    {
        _logger.LogInformation("---------- Specification Pattern ----------");

        // Get repository
        var productRepo = _unitOfWork.Repository<Product>();
        
        // Create specification for products with price > 50
        var expensiveProductsSpec = new SpecificationBuilder<Product>(p => p.Price > 50m);
        
        // Get products by specification
        var expensiveProducts = await productRepo.GetAllAsync(expensiveProductsSpec);
        _logger.LogInformation($"Found {expensiveProducts.Count} expensive products (price > 50)");
        
        // Create specification with ordering and include
        var orderedProductsSpec = new SpecificationBuilder<Product>()
            .Include(p => p.Category)
            .OrderByDescending(p => p.Price);
        
        // Get products with ordering and include
        var orderedProducts = await productRepo.GetAllAsync(orderedProductsSpec);
        
        _logger.LogInformation("Products ordered by price (descending):");
        foreach (var product in orderedProducts)
        {
            _logger.LogInformation($"- {product.Name}: ${product.Price} (Category: {product.Category?.Name})");
        }
        
        // Create paged specification
        int pageNumber = 1;
        int pageSize = 3;
        var pagedProductsSpec = new SpecificationBuilder<Product>()
            .Page(pageNumber, pageSize);
        
        // Get paged products
        var pagedProducts = await productRepo.GetPagedAsync(pagedProductsSpec, pageNumber, pageSize);
        
        _logger.LogInformation($"Paged products (Page {pagedProducts.Page} of {pagedProducts.TotalPages}, Items: {pagedProducts.Items.Count}, Total: {pagedProducts.TotalCount})");
        foreach (var product in pagedProducts.Items)
        {
            _logger.LogInformation($"- {product.Name}");
        }
    }

    private async Task CachedRepositoryAsync()
    {
        _logger.LogInformation("---------- Cached Repository ----------");

        // Get cached repository
        var cachedProductRepo = _unitOfWork.CachedRepository<Product>();
        
        // First call will hit the database
        _logger.LogInformation("First call (cache miss):");
        var products = await cachedProductRepo.GetAllAsync();
        _logger.LogInformation($"Found {products.Count} products");
        
        // Second call will hit the cache
        _logger.LogInformation("Second call (cache hit):");
        products = await cachedProductRepo.GetAllAsync();
        _logger.LogInformation($"Found {products.Count} products");
        
        // Add new product
        var newProduct = new Product
        {
            Name = "New Cached Product",
            Price = 199.99m,
            Description = "A test product added through the cached repository",
            CategoryId = products.First().CategoryId
        };
        
        var addedProduct = await cachedProductRepo.AddAsync(newProduct);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Added new product with ID: {addedProduct.Id}");
        
        // Clear cache
        await cachedProductRepo.ClearCacheAsync();
        _logger.LogInformation("Cache cleared");
        
        // This call will hit the database again
        _logger.LogInformation("Call after cache cleared (cache miss):");
        products = await cachedProductRepo.GetAllAsync();
        _logger.LogInformation($"Found {products.Count} products");
    }

    private async Task TransactionExampleAsync()
    {
        _logger.LogInformation("---------- Transaction Example ----------");

        try
        {
            // Begin transaction
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            
            // Get repository
            var categoryRepo = _unitOfWork.Repository<Category>();
            var productRepo = _unitOfWork.Repository<Product>();
            
            // Add new category
            var newCategory = new Category { Name = "Music" };
            await categoryRepo.AddAsync(newCategory);
            await _unitOfWork.SaveChangesAsync();
            
            // Add new product with new category
            var newProduct = new Product
            {
                Name = "Guitar",
                Price = 299.99m,
                Description = "Acoustic guitar",
                CategoryId = newCategory.Id
            };
            
            await productRepo.AddAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();
            
            // Add another product with invalid category (simulating an error)
            /*
            var invalidProduct = new Product
            {
                Name = "Piano",
                Price = 1999.99m,
                Description = "Grand piano",
                CategoryId = 999 // Invalid category ID
            };
            
            await productRepo.AddAsync(invalidProduct);
            await _unitOfWork.SaveChangesAsync();
            */
            
            // If everything is OK, commit transaction
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation("Transaction committed");
            
            // Verify new category and product
            var musicCategory = await categoryRepo.GetFirstOrDefaultAsync(
                new SpecificationBuilder<Category>(c => c.Name == "Music"));
            
            var guitarsSpec = new SpecificationBuilder<Product>(p => p.Name == "Guitar");
            var guitar = await productRepo.GetFirstOrDefaultAsync(guitarsSpec);
            
            _logger.LogInformation($"Added new category: {musicCategory?.Name}");
            _logger.LogInformation($"Added new product: {guitar?.Name} (${guitar?.Price})");
        }
        catch (Exception ex)
        {
            // If there's an error, rollback transaction
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError($"Transaction rolled back: {ex.Message}");
        }
    }
}