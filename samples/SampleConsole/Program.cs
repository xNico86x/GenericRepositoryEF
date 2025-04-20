using GenericRepositoryEF.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleConsole.Data;
using SampleConsole.Models;

// Create the host
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Add database context
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=console.db";
        services.AddDbContext<ConsoleDbContext>(options => 
            options.UseSqlite(connectionString));

        // Register Generic Repository services
        services.AddGenericRepository(options => {
            options.RegisterDbContext<ConsoleDbContext>();
            options.UseCaching = false; // We don't need caching for this console app
        });
    })
    .Build();

// Create database if it doesn't exist
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ConsoleDbContext>();
    dbContext.Database.EnsureCreated();

    // Add sample data if the database is empty
    if (!await dbContext.Customers.AnyAsync())
    {
        Console.WriteLine("Adding sample customers...");
        
        var customers = new List<Customer>
        {
            new Customer 
            { 
                Name = "John Doe", 
                Email = "john@example.com", 
                PhoneNumber = "555-1234" 
            },
            new Customer 
            { 
                Name = "Jane Smith", 
                Email = "jane@example.com", 
                PhoneNumber = "555-5678" 
            },
            new Customer 
            { 
                Name = "Bob Johnson", 
                Email = "bob@example.com", 
                PhoneNumber = "555-9012" 
            }
        };
        
        await dbContext.Customers.AddRangeAsync(customers);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Sample customers added successfully!");
    }
}

// Let's work with the repository
using (var scope = host.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<GenericRepositoryEF.Core.Interfaces.IRepository<Customer>>();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<GenericRepositoryEF.Core.Interfaces.IUnitOfWork>();

    Console.WriteLine("\n===== Generic Repository Pattern Demo =====\n");

    // Fetch all customers
    Console.WriteLine("Fetching all customers:");
    var allCustomers = await repository.ListAsync();
    foreach (var customer in allCustomers)
    {
        Console.WriteLine($"- {customer.Id}: {customer.Name} ({customer.Email})");
    }

    // Add a new customer
    Console.WriteLine("\nAdding a new customer...");
    var newCustomer = new Customer
    {
        Name = "Alice Brown",
        Email = "alice@example.com",
        PhoneNumber = "555-3456"
    };
    
    await repository.AddAsync(newCustomer);
    await unitOfWork.SaveChangesAsync();
    Console.WriteLine($"Added customer: {newCustomer.Id}: {newCustomer.Name}");

    // Update a customer
    Console.WriteLine("\nUpdating the first customer...");
    var firstCustomer = allCustomers.First();
    firstCustomer.PhoneNumber = "555-UPDATED";
    
    await repository.UpdateAsync(firstCustomer);
    await unitOfWork.SaveChangesAsync();
    Console.WriteLine($"Updated customer: {firstCustomer.Id}: {firstCustomer.Name} with new phone: {firstCustomer.PhoneNumber}");

    // Find a customer by id
    Console.WriteLine("\nFinding customer by id:");
    var foundCustomer = await repository.GetByIdAsync(2);
    if (foundCustomer != null)
    {
        Console.WriteLine($"Found: {foundCustomer.Id}: {foundCustomer.Name} ({foundCustomer.Email})");
    }

    // Use a specification
    Console.WriteLine("\nFinding customers by specification (email containing 'example.com'):");
    var byEmailSpec = new GenericRepositoryEF.Core.Specifications.BaseSpecification<Customer>(
        c => c.Email.Contains("example.com"));
    
    var filteredCustomers = await repository.ListAsync(byEmailSpec);
    foreach (var customer in filteredCustomers)
    {
        Console.WriteLine($"- {customer.Id}: {customer.Name} ({customer.Email})");
    }

    Console.WriteLine("\nDemo completed!");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();