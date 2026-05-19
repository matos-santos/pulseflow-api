# 🏗️ Infrastructure Layer - PulseFlow API

Camada de infraestrutura seguindo **Clean Architecture** com Entity Framework Core e PostgreSQL.

---

## 📁 Estrutura do Projeto

```
src/PulseFlow.Infrastructure/
├── Persistence/
│   ├── ApplicationDbContext.cs              # DbContext principal
│   ├── Configurations/                      # Entity Configurations (Fluent API)
│   │   └── SampleEntityConfiguration.cs
│   ├── Interceptors/                        # EF Core Interceptors
│   │   └── AuditableEntityInterceptor.cs    # Auditoria automática
│   └── Migrations/                          # EF Core Migrations
│       ├── 20260518202432_InitialCreate.cs
│       ├── 20260518202432_InitialCreate.Designer.cs
│       └── ApplicationDbContextModelSnapshot.cs
├── Repositories/
│   ├── BaseRepository.cs                    # Repository genérico
│   ├── UnitOfWork.cs                       # Unit of Work pattern
│   ├── ISampleRepository.cs                # Interface específica
│   └── SampleRepository.cs                 # Implementação específica
└── DependencyInjection.cs                  # Configuração de IoC
```

---

## 🎯 Responsabilidades

Esta camada é responsável por:

- ✅ Acesso ao banco de dados (Entity Framework Core)
- ✅ Implementação de repositories
- ✅ Gerenciamento de transações (Unit of Work)
- ✅ Migrations do banco de dados
- ✅ Configurações de entidades (Fluent API)
- ✅ Interceptors para cross-cutting concerns (auditoria, etc)

---

## 🔧 Tecnologias Utilizadas

- **Entity Framework Core 9.0** - ORM
- **Npgsql.EntityFrameworkCore.PostgreSQL 9.0.2** - Provider PostgreSQL
- **Microsoft.EntityFrameworkCore.Design** - EF Core Tools

---

## 🚀 Como Usar

### 1. Registrar no Program.cs

```csharp
using PulseFlow.Infrastructure;

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);
```

### 2. Connection String

Configure no `appsettings.json`:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Host=localhost;Port=5433;Database=pulseflow_db;Username=pulseflow_user;Password=pulseflow_password"
  }
}
```

### 3. Usar Repositories

```csharp
public class MyService
{
	private readonly ISampleRepository _sampleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public MyService(ISampleRepository sampleRepository, IUnitOfWork unitOfWork)
	{
		_sampleRepository = sampleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<SampleEntity> CreateSampleAsync(string name)
	{
		var sample = new SampleEntity(name, "Description");

		await _sampleRepository.AddAsync(sample);
		await _unitOfWork.SaveChangesAsync();

		return sample;
	}
}
```

---

## 📦 Padrões Implementados

### 1. Repository Pattern

**Interface genérica:**

```csharp
public interface IRepository<TEntity, TKey>
{
	Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, ...);
	Task<TEntity> AddAsync(TEntity entity, ...);
	Task UpdateAsync(TEntity entity, ...);
	Task DeleteAsync(TEntity entity, ...);
	IQueryable<TEntity> AsQueryable();
}
```

**Implementação específica:**

```csharp
public interface ISampleRepository : IRepository<SampleEntity, Guid>
{
	Task<IReadOnlyList<SampleEntity>> GetActiveAsync(...);
	Task<SampleEntity?> GetByNameAsync(string name, ...);
}
```

### 2. Unit of Work Pattern

```csharp
public interface IUnitOfWork : IDisposable
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	Task BeginTransactionAsync(CancellationToken cancellationToken = default);
	Task CommitTransactionAsync(CancellationToken cancellationToken = default);
	Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
```

**Uso com transação:**

```csharp
await _unitOfWork.BeginTransactionAsync();
try
{
	await _repository1.AddAsync(entity1);
	await _repository2.AddAsync(entity2);

	await _unitOfWork.CommitTransactionAsync();
}
catch
{
	await _unitOfWork.RollbackTransactionAsync();
	throw;
}
```

### 3. Specification Pattern (Base)

Use `AsQueryable()` para queries complexas:

```csharp
var activeUsers = await _userRepository
	.AsQueryable()
	.Where(u => u.IsActive)
	.Include(u => u.Orders)
	.OrderByDescending(u => u.CreatedAt)
	.Take(10)
	.ToListAsync();
```

---

## 🔍 Features Implementadas

### ✅ Auditoria Automática

O `AuditableEntityInterceptor` preenche automaticamente:

- `CreatedAt` / `CreatedBy` - ao inserir
- `UpdatedAt` / `UpdatedBy` - ao atualizar
- `DeletedAt` / `DeletedBy` / `IsDeleted` - ao deletar (soft delete)

```csharp
public class SampleEntity : AuditableEntity<Guid>
{
	// CreatedAt, CreatedBy, UpdatedAt, UpdatedBy são preenchidos automaticamente!
}
```

### ✅ Soft Delete Automático

Entidades que implementam `ISoftDeletable` são marcadas como deletadas em vez de removidas:

```csharp
public class Product : SoftDeletableEntity<Guid>
{
	// Ao chamar DbContext.Remove(), apenas IsDeleted = true
}

// Query filters aplicados globalmente
var products = await _productRepository.GetAllAsync();
// Retorna apenas produtos com IsDeleted = false
```

### ✅ Retry Policy

Configurado para falhas transientes do PostgreSQL:

```csharp
options.UseNpgsql(connectionString, npgsqlOptions =>
{
	npgsqlOptions.EnableRetryOnFailure(
		maxRetryCount: 5,
		maxRetryDelay: TimeSpan.FromSeconds(10),
		errorCodesToAdd: null);
});
```

### ✅ Migrations Automáticas (Development)

```csharp
if (app.Environment.IsDevelopment())
{
	await app.Services.ApplyMigrationsAsync();
}
```

---

## 📚 Guias Detalhados

- 📘 **[MIGRATIONS-GUIDE.md](MIGRATIONS-GUIDE.md)** - Como trabalhar com migrations
- 📗 **[CREATE-ENTITY-GUIDE.md](CREATE-ENTITY-GUIDE.md)** - Como criar novas entidades

---

## 🧪 Testando

### Testar Connection String

```powershell
dotnet ef dbcontext info --project src\PulseFlow.Infastructure\Infastructure.csproj --startup-project src\PulseFlow.Api\Api.csproj
```

### Ver Schema do Banco

```powershell
dotnet ef dbcontext script --project src\PulseFlow.Infastructure\Infastructure.csproj --startup-project src\PulseFlow.Api\Api.csproj
```

### Conectar via PostgreSQL Client

```powershell
docker exec -it pulseflow-postgres psql -U pulseflow_user -d pulseflow_db
```

---

## 🔐 Configurações de Segurança

### Development

```json
{
  "Logging": {
	"EnableSensitiveDataLogging": true,
	"EnableDetailedErrors": true
  }
}
```

### Production

```json
{
  "Logging": {
	"EnableSensitiveDataLogging": false,
	"EnableDetailedErrors": false
  }
}
```

---

## 🎓 Princípios Aplicados

### ✅ SOLID

- **S**ingle Responsibility: Cada repository tem responsabilidade única
- **O**pen/Closed: Extensível via herança de BaseRepository
- **L**iskov Substitution: Interfaces bem definidas
- **I**nterface Segregation: IRepository, IUnitOfWork separados
- **D**ependency Inversion: Dependências via interfaces

### ✅ Clean Architecture

```
Domain (Core)
	↑
Infrastructure (Adapter)
	↑
API (Presentation)
```

- Domain não conhece Infrastructure
- Infrastructure implementa abstrações do Domain
- API depende de ambos mas usa apenas interfaces

### ✅ DDD (Domain-Driven Design)

- Entities com comportamento rico
- Repositories como coleções em memória
- Unit of Work para consistência transacional
- Aggregate roots controlam limites transacionais

---

## 📊 Diagrama de Dependências

```
┌─────────────────────────────────────┐
│         PulseFlow.Api               │
│  (Presentation Layer)               │
└──────────┬──────────────────────────┘
		   │ depends on
		   ↓
┌─────────────────────────────────────┐
│    PulseFlow.Infrastructure         │
│  (Infrastructure Layer)             │
│  • Persistence                      │
│  • Repositories                     │
│  • EF Core Configs                  │
└──────────┬──────────────────────────┘
		   │ implements
		   ↓
┌─────────────────────────────────────┐
│      PulseFlow.Domain               │
│  (Domain Layer)                     │
│  • Entities                         │
│  • Interfaces (IRepository, etc)    │
│  • Business Rules                   │
└─────────────────────────────────────┘
```

---

## 🚨 Troubleshooting

### Erro: "No connection string"

Verifique se está no `appsettings.json` com o nome `DefaultConnection`.

### Erro: "Could not connect to the database"

```powershell
cd commons
docker-compose up -d postgres
docker ps
```

### Erro: "Pending migrations"

```powershell
dotnet ef database update --project src\PulseFlow.Infastructure\Infastructure.csproj --startup-project src\PulseFlow.Api\Api.csproj
```

### Performance lenta

- Verifique se há indexes nos campos de busca
- Use `.AsNoTracking()` para queries read-only
- Considere paginação para listas grandes

---

## 📈 Próximos Passos

- [ ] Implementar ICurrentUserService para auditoria com usuário real
- [ ] Adicionar health check para banco de dados
- [ ] Implementar caching (Redis)
- [ ] Adicionar logging de queries lentas
- [ ] Implementar outbox pattern para eventos
- [ ] Adicionar testes de integração

---

## 🔗 Links Úteis

- [EF Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Npgsql EF Core Provider](https://www.npgsql.org/efcore/)
- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)
- [Unit of Work Pattern](https://martinfowler.com/eaaCatalog/unitOfWork.html)
