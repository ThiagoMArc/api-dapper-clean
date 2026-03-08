# 📘 ApiDapperClean

API robusta em **.NET 10** com **Clean Architecture**, **Dapper** e **PostgreSQL**.

**Stack**: .NET 10 LTS | Dapper 2.1.21 | PostgreSQL | xUnit | Serilog
**Status**: ✅ Pronto para desenvolvimento

---

## ✨ Features

- ✅ 4 Camadas de Arquitetura
- ✅ Exemplo completo (Product CRUD)
- ✅ Serviços segregados por operação
- ✅ Testes Unitários
- ✅ Documentação Scalar (OpenAPI)
- ✅ Docker + docker-compose
- ✅ Validação de dados
- ✅ Logging centralizado

---

## 🚀 Quick Start

```bash
dotnet restore && dotnet build              # Build
dotnet run --project src/ApiDapperClean.Api # Executar

# Acessar:
# Docs: http://localhost:8080/scalar/v1
# API:  http://localhost:8080/api/v1/products?page=1&pageSize=10
```

---

## 📁 Estrutura (4 Camadas)

```
src/
├── ApiDapperClean.Api/                # 🎨 HTTP (Controllers, Middleware)
├── ApiDapperClean.Application/        # 💼 Serviços, DTOs, Validators
├── ApiDapperClean.Domain/             # 🎯 Entities, Interfaces
└── ApiDapperClean.Infrastructure/     # 🔧 Repositories, Database

tests/
└── ApiDapperClean.UnitTests/          # 🧪 33 Testes
```

---

## 🏗️ Arquitetura

| Camada             | Responsabilidade                        |
| ------------------ | --------------------------------------- |
| **Domain**         | Entities, Interfaces, Regras de negócio |
| **Application**    | Services, DTOs, Validators, Mappers     |
| **Infrastructure** | Repositories, Database, Migrations      |
| **API**            | Controllers, Middlewares                |

---

## 🔌 API Endpoints

| Método | Endpoint         | Descrição            | Parâmetros            |
| ------ | ---------------- | -------------------- | --------------------- |
| GET    | `/products`      | Listar com paginação | `?page=1&pageSize=10` |
| GET    | `/products/{id}` | Obter por ID         | -                     |
| POST   | `/products`      | Criar                | -                     |
| PUT    | `/products/{id}` | Atualizar            | -                     |
| DELETE | `/products/{id}` | Deletar              | -                     |

**Base URL**: `http://localhost:8080/api/v1`

**Exemplos:**

```bash
# Listar com paginação (padrão: page=1, pageSize=10)
curl "http://localhost:8080/api/v1/products?page=1&pageSize=10"

# Criar
curl -X POST http://localhost:8080/api/v1/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Notebook","description":"High-end","price":2500,"stock":10}'

# Obter por ID
curl http://localhost:8080/api/v1/products/{id}

# Atualizar
curl -X PUT http://localhost:8080/api/v1/products/{id} \
  -H "Content-Type: application/json" \
  -d '{"name":"Updated","description":"Desc","price":3000,"stock":15}'

# Deletar
curl -X DELETE http://localhost:8080/api/v1/products/{id}
```

---

## 💻 Comandos

### Desenvolvimento

```bash
dotnet run --project src/ApiDapperClean.Api        # Executar
dotnet watch --project src/ApiDapperClean.Api run  # Auto-reload
dotnet build                                        # Build
```

### Testes

```bash
dotnet test                                    # Todos
dotnet test --filter "ProductServiceTests"    # Específico
dotnet watch test                              # Com watch
```

### Docker

```bash
docker compose up -d        # Iniciar
docker compose ps          # Status
docker compose logs -f api # Logs
docker compose down        # Parar
```

---

## 🎯 Stack Tecnológico

| Tecnologia       | Versão   | Uso       |
| ---------------- | -------- | --------- |
| .NET             | 10.0 LTS | Framework |
| ASP.NET Core     | 10.0     | Web       |
| Dapper           | 2.1.21   | ORM       |
| PostgreSQL       | 12+      | Database  |
| FluentValidation | 11.9.1   | Validação |
| Serilog          | 8.0.0    | Logging   |
| Scalar           | latest   | API Docs  |
| xUnit            | 2.7.0    | Testes    |
| Moq              | latest   | Mocking   |

---

## 📊 Padrões Implementados

- ✅ Clean Architecture (4 camadas)
- ✅ Repository Pattern
- ✅ Service Segregation (Interface por operação)
- ✅ Dependency Injection
- ✅ Result Pattern
- ✅ Mapper Pattern
- ✅ SOLID Principles
- ✅ CancellationToken Support
- ✅ Pagination Support
- ✅ Soft Delete

---

## 🏛️ Arquitetura de Serviços

Serviços separados por operação

```
IGetProductsService          → GetPagedAsync(page, pageSize)
IGetProductByIdService       → GetByIdAsync(id)
ICreateProductService        → CreateAsync(dto)
IUpdateProductService        → UpdateAsync(id, dto)
IDeleteProductService        → DeleteAsync(id)
```

Cada serviço é injetado no controller e responsável por uma operação específica, facilitando testes unitários e manutenção.

---

## 📈 Testes

```bash
dotnet test                              # Todos
dotnet test --filter "ProductService"    # Serviço
dotnet watch test                         # Com watch
```

**Cobertura**:

- Padrão AAA (Arrange, Act, Assert)
- Mocking com Moq
- Shouldly
