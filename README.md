# GenericRepositoryEF

GenericRepositoryEF es una biblioteca .NET 8 que implementa el patrón Repository y Unit of Work utilizando Entity Framework Core. La biblioteca proporciona una capa de acceso a datos flexible y reutilizable.

## Características

- **Implementación Genérica de Repository**: Compatible con cualquier entidad
- **Unit of Work**: Gestión de transacciones con soporte para diferentes niveles de aislamiento
- **Especificaciones**: Patrón Specification para consultas complejas y composición
- **Caché**: Soporte para caché distribuido para mejorar el rendimiento
- **Auditoría**: Seguimiento automático de cambios (creación, modificación, eliminación)
- **Eliminación Suave**: Soporte para borrado lógico
- **Soporte para Múltiples Bases de Datos**: SQL Server, PostgreSQL, SQLite y en memoria (para pruebas)
- **Inyección de Dependencias**: Configuración sencilla con Microsoft.Extensions.DependencyInjection
- **Manejo de Excepciones**: Excepciones específicas para diferentes escenarios

## Estructura de la Solución

La solución está dividida en tres proyectos principales:

- **GenericRepositoryEF.Core**: Interfaces y abstracciones
- **GenericRepositoryEF.Infrastructure**: Implementaciones de las interfaces
- **GenericRepositoryEF.Extensions**: Extensiones para configuración e inyección de dependencias

## Uso Básico

### Configuración

```csharp
// En Program.cs o Startup.cs
services.AddGenericRepository<YourDbContext>(options =>
{
    options.AddAuditInterceptor = true;
    options.AddSoftDeleteInterceptor = true;
    options.AddDateTimeService = true;
});

// O con más opciones
services.AddGenericRepository<YourDbContext>(options =>
{
    options.AddAuditInterceptor = true;
    options.AddSoftDeleteInterceptor = true;
}, dbOptions =>
{
    dbOptions.UseGenericRepositorySqlServer(Configuration.GetConnectionString("DefaultConnection"));
});
```

### Definir una Entidad

```csharp
public class Customer : IEntityWithKey<int>, IAuditableEntity, ISoftDeleteWithUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    // IAuditableEntity
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }
    
    // ISoftDeleteWithUser
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
```

### Uso del Repositorio

```csharp
public class CustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CustomerService(IRepository<Customer> customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Customer> GetByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }
    
    public async Task<IReadOnlyList<Customer>> GetAllAsync()
    {
        return await _customerRepository.ListAllAsync();
    }
    
    public async Task<IReadOnlyList<Customer>> GetCustomersWithSpecification()
    {
        var spec = new BaseSpecification<Customer>(x => x.Email.Contains("example.com"))
            .AddInclude(x => x.Orders)
            .AddOrderBy(x => x.Name);
            
        return await _customerRepository.ListAsync(spec);
    }
    
    public async Task AddCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateCustomerAsync(Customer customer)
    {
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer != null)
        {
            _customerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
```

## Patrones de Diseño Implementados

- **Repository Pattern**: Abstracción del acceso a datos
- **Unit of Work Pattern**: Gestión de transacciones
- **Specification Pattern**: Consultas complejas y reutilizables
- **Decorator Pattern**: Para implementar funcionalidades como caché
- **Interceptor Pattern**: Para auditoría y borrado lógico

## Licencia

Este proyecto está licenciado bajo la Licencia MIT.