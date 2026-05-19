# PulseFlow API - Docker Configuration

Este diretório contém os arquivos de configuração Docker para rodar o projeto PulseFlow API com PostgreSQL.

## Estrutura

```
commons/
├── Dockerfile              # Imagem Docker da aplicação .NET 9
├── docker-compose.yml      # Orquestração dos serviços
├── .dockerignore          # Arquivos ignorados no build
├── postgres/
│   └── init.sql           # Script de inicialização do PostgreSQL
└── README.md              # Este arquivo
```

## Serviços

### 1. PostgreSQL (postgres)
- **Imagem**: `postgres:17-alpine`
- **Container**: `pulseflow-postgres`
- **Porta**: `5433:5432`
- **Credenciais**:
  - Usuário: `pulseflow_user`
  - Senha: `pulseflow_password`
  - Database: `pulseflow_db`

### 2. PulseFlow API (api)
- **Imagem**: Build multi-stage com .NET 9
- **Container**: `pulseflow-api`
- **Porta**: `5000:8080`
- **Connection String**: Configurada via variável de ambiente

## Como Usar

### Subir os serviços

```bash
# A partir da raiz do projeto
cd commons
docker-compose up -d
```

### Verificar logs

```bash
# Logs de todos os serviços
docker-compose logs -f

# Logs apenas da API
docker-compose logs -f api

# Logs apenas do PostgreSQL
docker-compose logs -f postgres
```

### Parar os serviços

```bash
docker-compose down
```

### Parar e remover volumes (limpar dados)

```bash
docker-compose down -v
```

### Rebuild da aplicação

```bash
docker-compose up -d --build
```

## Acessar a Aplicação

- **API**: http://localhost:5000
- **API Documentation (Scalar)**: http://localhost:5000/scalar/v1
- **Health Check**: http://localhost:5000/health
- **PostgreSQL**: localhost:5433

## Conectar ao PostgreSQL

### Via Docker

```bash
docker exec -it pulseflow-postgres psql -U pulseflow_user -d pulseflow_db
```

### Via Cliente PostgreSQL

```bash
psql -h localhost -p 5433 -U pulseflow_user -d pulseflow_db
```

### Connection String para ferramentas

```
Host=localhost;Port=5433;Database=pulseflow_db;Username=pulseflow_user;Password=pulseflow_password
```

## Variáveis de Ambiente

Você pode customizar as variáveis no `docker-compose.yml`:

### PostgreSQL
- `POSTGRES_USER`: Usuário do banco
- `POSTGRES_PASSWORD`: Senha do banco
- `POSTGRES_DB`: Nome do banco
- `PGDATA`: Diretório dos dados

### API
- `ASPNETCORE_ENVIRONMENT`: Ambiente da aplicação (Development/Production)
- `ASPNETCORE_URLS`: URLs de binding
- `ConnectionStrings__DefaultConnection`: String de conexão com o PostgreSQL

## Health Checks

Ambos os serviços possuem health checks configurados:

- **PostgreSQL**: Verifica se o banco está pronto com `pg_isready`
- **API**: Verifica o endpoint `/health`

## Volumes

- `postgres_data`: Persiste os dados do PostgreSQL

## Network

Os serviços estão conectados na rede `pulseflow-network` permitindo comunicação interna.

## Notas

- O PostgreSQL usa a imagem Alpine para ser mais leve
- A API aguarda o PostgreSQL estar saudável antes de iniciar
- O Dockerfile usa multi-stage build para otimizar o tamanho da imagem
- A aplicação roda com um usuário não-root por segurança
- Os logs são configurados via Serilog
