# WebApi - ASP.NET Core Study Project

Projeto de estudo de Web API com ASP.NET Core 10.0, implementando autenticação JWT, Entity Framework Core, versionamento de API e boas práticas de desenvolvimento.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

## Sobre o Projeto

Este é um projeto de estudo focado em aprender e praticar os principais conceitos de desenvolvimento de APIs RESTful com ASP.NET Core. O projeto simula um sistema de gerenciamento de funcionários com funcionalidades de CRUD, autenticação JWT, upload de arquivos e versionamento de API.

### Objetivo Educacional

O objetivo principal é consolidar conhecimentos em:
- Desenvolvimento de APIs RESTful modernas
- Autenticação e autorização
- Persistência de dados com Entity Framework Core
- Arquitetura em camadas e padrões de projeto
- Documentação de APIs
- Containerização com Docker

## Tecnologias Utilizadas

- **.NET 10.0** - Framework principal
- **ASP.NET Core Web API** - Framework para criação da API
- **Entity Framework Core 10.0.3** - ORM para acesso a dados
- **PostgreSQL 16** - Banco de dados relacional (containerizado)
- **Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0** - Provider do PostgreSQL para EF Core
- **JWT Authentication** - Autenticação baseada em tokens
  - `Microsoft.AspNetCore.Authentication.JwtBearer 10.0.3`
  - `System.IdentityModel.Tokens.Jwt 8.16.0`
- **AutoMapper 12.0.1** - Mapeamento objeto-objeto (DTOs)
- **Swagger/OpenAPI** - Documentação interativa da API
  - `Swashbuckle.AspNetCore 10.1.2`
- **API Versioning** - Gerenciamento de versões da API
  - `Asp.Versioning.Mvc 7.1.1`
  - `Asp.Versioning.Mvc.ApiExplorer 7.1.0`
- **Docker Compose** - Orquestração de containers

## Arquitetura e Padrões

O projeto segue uma arquitetura em camadas, separando responsabilidades:

```
WebApi/
├── Domain/              # Camada de Domínio
│   ├── Models/          # Entidades (Employee, Company)
│   ├── DTOs/            # Data Transfer Objects
│   └── Interfaces/      # Contratos de repositórios
├── Application/         # Camada de Aplicação
│   ├── Services/        # Serviços (TokenService)
│   ├── ViewModels/      # Modelos de entrada
│   ├── Mapping/         # Configuração do AutoMapper
│   └── Swagger/         # Configuração do Swagger
├── Infra/              # Camada de Infraestrutura
│   ├── Repositories/    # Implementação dos repositórios
│   └── ConnectionContext.cs  # Contexto do Entity Framework
├── Controllers/         # Controladores da API
│   ├── v1/             # Versão 1 (deprecated)
│   └── v2/             # Versão 2 (atual)
├── Migrations/          # Migrações do EF Core
└── Storage/             # Armazenamento local de arquivos
```

### Padrões Implementados

- **Repository Pattern** - Abstração da camada de acesso a dados
- **Dependency Injection** - Inversão de controle e injeção de dependências
- **DTO Pattern** - Separação entre modelos de domínio e transferência de dados
- **Separation of Concerns** - Separação clara entre camadas
- **API Versioning** - Controle de versões da API (v1 deprecated, v2 ativa)

## Funcionalidades Implementadas

- ✅ CRUD completo de funcionários (Employee)
- ✅ Autenticação JWT com token bearer
- ✅ Upload de fotos de funcionários
- ✅ Download de fotos armazenadas
- ✅ Paginação de resultados
- ✅ Versionamento de API (v1 e v2)
- ✅ Documentação Swagger com autenticação Bearer
- ✅ Health Check endpoint
- ✅ Tratamento de erros global
- ✅ Migrations do Entity Framework Core

## Pré-requisitos

Para executar este projeto, você precisará ter instalado:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started) e Docker Compose
- Editor de código (Visual Studio, VS Code ou Rider)
- [Git](https://git-scm.com/)

## Configuração e Instalação

### Passo 1: Clonar o repositório

```bash
git clone <url-do-repositório>
cd WebApi
```

### Passo 2: Subir o banco de dados PostgreSQL

O projeto utiliza PostgreSQL em container Docker. Execute:

```bash
docker-compose up -d
```

Isso criará um container com as seguintes configurações:
- **Container**: postgres-db
- **Porta**: 5432
- **Database**: app_db
- **Usuário**: postgres
- **Senha**: postgres

### Passo 3: Restaurar dependências

```bash
dotnet restore
```

### Passo 4: Aplicar as migrations

```bash
dotnet ef database update
```

Isso criará as tabelas `employee` e `company` no banco de dados.

### Passo 5: Executar a aplicação

```bash
dotnet run
```

A API estará disponível em:
- **HTTPS**: `https://localhost:7198`
- **HTTP**: `http://localhost:5116`

## Documentação da API

### Swagger UI

Acesse a documentação interativa em: `https://localhost:7198/swagger`

O Swagger permite testar todos os endpoints diretamente pelo navegador, incluindo suporte para autenticação Bearer token.

### Endpoints Principais

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/` | Health check | Não |
| POST | `/api/v1/auth` | Autenticação (obter token JWT) | Não |
| POST | `/api/v{version}/employee` | Criar novo funcionário | Sim |
| GET | `/api/v{version}/employee` | Listar funcionários (com paginação) | Sim |
| GET | `/api/v{version}/employee/{id}` | Buscar funcionário por ID | Sim |
| POST | `/api/v{version}/employee/{id}/download` | Download da foto do funcionário | Sim |

> **Nota**: `{version}` pode ser `v1` ou `v2`. Recomenda-se usar `v2` pois `v1` está marcada como deprecated.

### Autenticação

O projeto utiliza autenticação JWT (JSON Web Token). Para acessar endpoints protegidos:

#### Credenciais de Teste

- **Usuário**: `leonardo`
- **Senha**: `123456`
- **Validade do Token**: 3 horas

#### Exemplo de Autenticação

**1. Obter o token:**

```bash
curl -X POST "https://localhost:7198/api/v1/auth?username=leonardo&password=123456"
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**2. Usar o token nas requisições:**

```bash
curl -H "Authorization: Bearer {seu-token-aqui}" \
     "https://localhost:7198/api/v2/employee?pageNumber=1&pageQuantity=10"
```

#### Autenticação no Swagger

1. Clique no botão **Authorize** no topo da página
2. Digite: `Bearer {seu-token-aqui}`
3. Clique em **Authorize**
4. Agora você pode testar os endpoints protegidos

### Exemplos de Requisições

#### Criar Funcionário

```bash
curl -X POST "https://localhost:7198/api/v2/employee" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: multipart/form-data" \
  -F "Name=João Silva" \
  -F "Age=30" \
  -F "Photo=@/caminho/para/foto.jpg"
```

#### Listar Funcionários

```bash
curl -X GET "https://localhost:7198/api/v2/employee?pageNumber=1&pageQuantity=10" \
  -H "Authorization: Bearer {token}"
```

#### Buscar por ID

```bash
curl -X GET "https://localhost:7198/api/v2/employee/1" \
  -H "Authorization: Bearer {token}"
```

## Configurações

### Connection String

A string de conexão está configurada em `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=app_db;Username=postgres;Password=postgres"
  }
}
```

### JWT Secret Key

⚠️ **IMPORTANTE**: O secret key está hardcoded no arquivo `Key.cs` apenas para fins de estudo. 

**Em ambiente de produção, você deve:**
- Usar variáveis de ambiente
- Utilizar Azure Key Vault, AWS Secrets Manager ou similar
- Nunca commitar secrets no repositório

## Modelos de Dados

### Employee (Funcionário)

```csharp
{
    "id": 1,                    // int (Primary Key)
    "name": "João Silva",       // string (obrigatório)
    "age": 30,                  // int
    "photo": "Storage/foto.jpg" // string (nullable)
}
```

### Company (Empresa)

```csharp
{
    "id": 1,              // int (Primary Key)
    "name": "Acme Corp"   // string (obrigatório)
}
```

## Conceitos de Aprendizado

Este projeto aborda os seguintes conceitos importantes:

- **Criação de Web APIs RESTful** com ASP.NET Core
- **Entity Framework Core** com abordagem Code First
- **Migrations** para controle de versão do banco de dados
- **Autenticação e Autorização** com JWT Bearer tokens
- **Injeção de Dependências** nativa do ASP.NET Core
- **Repository Pattern** para abstração de acesso a dados
- **AutoMapper** para mapeamento automático entre objetos
- **Versionamento de APIs** para manter compatibilidade
- **Documentação com Swagger/OpenAPI**
- **Containerização** de serviços com Docker
- **Upload e download de arquivos** em APIs
- **Paginação** para otimização de consultas
- **Arquitetura em camadas** (Domain, Application, Infrastructure)
- **Separation of Concerns** (SoC)
- **Clean Code** e boas práticas de nomenclatura

## Melhorias Futuras

Sugestões para expandir este projeto de estudo:

### Segurança
- [ ] Implementar autorização baseada em roles/claims
- [ ] Adicionar refresh tokens
- [ ] Implementar rate limiting
- [ ] Configurar CORS adequadamente

### Testes
- [ ] Testes unitários (xUnit)
- [ ] Testes de integração
- [ ] Testes de carga (JMeter/K6)

### Arquitetura
- [ ] Implementar CQRS pattern
- [ ] Adicionar MediatR para mediação de comandos
- [ ] Implementar eventos de domínio

### Validação e Qualidade
- [ ] Adicionar FluentValidation
- [ ] Implementar logging estruturado (Serilog)
- [ ] Adicionar Health Checks mais robustos
- [ ] Implementar auditoria de entidades

### Performance
- [ ] Adicionar cache distribuído (Redis)
- [ ] Implementar cache de resposta HTTP
- [ ] Otimizar queries com projections

### DevOps
- [ ] Containerizar a aplicação completa
- [ ] CI/CD pipeline (GitHub Actions, Azure DevOps)
- [ ] Configurar ambiente de staging
- [ ] Implementar feature flags

### Funcionalidades
- [ ] Implementar soft delete
- [ ] Adicionar filtros e ordenação
- [ ] Implementar busca avançada
- [ ] Migrar storage para Azure Blob Storage ou AWS S3

## Comandos Úteis

### Entity Framework Core

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter para uma migration específica
dotnet ef database update NomeDaMigrationAnterior

# Remover última migration (não aplicada)
dotnet ef migrations remove

# Listar migrations
dotnet ef migrations list

# Gerar script SQL
dotnet ef migrations script
```

### .NET CLI

```bash
# Limpar e reconstruir
dotnet clean && dotnet build

# Executar em modo watch (hot reload)
dotnet watch run

# Executar testes
dotnet test

# Publicar aplicação
dotnet publish -c Release -o ./publish

# Verificar versão do .NET
dotnet --version

# Listar SDKs instalados
dotnet --list-sdks
```

### Docker

```bash
# Subir containers
docker-compose up -d

# Parar containers
docker-compose down

# Ver logs
docker-compose logs -f postgres

# Reiniciar container
docker-compose restart postgres

# Acessar o PostgreSQL
docker exec -it postgres-db psql -U postgres -d app_db
```

## Observações Técnicas

### API Versioning
- **v1** está marcada como `Deprecated = true`
- **v2** é a versão atual e recomendada
- O versionamento é feito via URL: `/api/v1/...` e `/api/v2/...`

### Swagger
- Configurado com suporte a autenticação Bearer token
- Documentação separada por versão da API
- Disponível apenas em ambiente de desenvolvimento

### Storage de Arquivos
- Fotos são salvas localmente na pasta `Storage/`
- Em produção, recomenda-se usar serviços de blob storage (Azure Blob, AWS S3, MinIO)
- Não há validação de tipo de arquivo (adicionar para produção)

### CORS
- CORS não está configurado atualmente
- Adicione configuração de CORS se precisar consumir a API de um frontend

### HTTPS
- Redirecionamento HTTPS configurado no pipeline
- Certificado de desenvolvimento é gerado automaticamente pelo .NET

### Exception Handling
- Middleware global de tratamento de erros configurado
- Endpoint `/error` para captura de exceções

## Licença

Este é um projeto de estudo e não possui uma licença específica. Sinta-se livre para usar como referência para seu aprendizado.

## Autor

Leonardo dos Santos Marcos

---

**Desenvolvido com foco em aprendizado e boas práticas de desenvolvimento .NET** 🚀
