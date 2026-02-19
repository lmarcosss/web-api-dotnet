# WebApi - ASP.NET Core Study Project

Projeto de estudo de Web API com ASP.NET Core 10.0, implementando autenticação JWT, Entity Framework Core, versionamento de API e boas práticas de desenvolvimento.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

## Sobre o Projeto

Este é um projeto de estudo focado em aprender e praticar os principais conceitos de desenvolvimento de APIs RESTful com ASP.NET Core. O projeto simula um sistema de gerenciamento de usuários com funcionalidades de CRUD, autenticação JWT (email e senha), upload de arquivos e versionamento de API.

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
- **AWS SDK S3** (`AWSSDK.S3` 4.0.18.6) - Armazenamento de arquivos em nuvem
- **BCrypt.Net-Next** (4.0.3) - Hash de senhas
- **Docker Compose** - Orquestração de containers

## Arquitetura e Padrões

O projeto segue uma arquitetura em camadas, separando responsabilidades:

```
WebApi/
├── Domain/              # Camada de Domínio
│   ├── Models/          # Entidades (User)
│   ├── DTOs/            # Data Transfer Objects
│   └── Interfaces/      # Contratos de repositórios
├── Application/         # Camada de Aplicação
│   ├── Services/        # UserService, TokenService, S3FileStorageService
│   ├── Services/Interfaces/  # IUserService, ITokenService, IFileStorageService
│   ├── ViewModel/        # Modelos de entrada
│   ├── Mapping/         # Configuração do AutoMapper
│   └── Swagger/         # Configuração do Swagger
├── Infra/              # Camada de Infraestrutura
│   ├── Repositories/    # Implementação dos repositórios
│   └── ConnectionContext.cs  # Contexto do Entity Framework
├── Settings/            # Configurações (JwtSettings via appsettings)
├── Controllers/         # Controladores da API
│   └── v1/             # Versão 1
└── Migrations/          # Migrações do EF Core
```

### Padrões Implementados

- **Repository Pattern** - Abstração da camada de acesso a dados
- **Application Services** - Orquestração entre repositório, storage e mapeamento (UserService, TokenService)
- **Dependency Injection** - Inversão de controle e injeção de dependências
- **DTO Pattern** - Separação entre modelos de domínio e transferência de dados
- **Separation of Concerns** - Separação clara entre camadas
- **API Versioning** - Controle de versões da API (v1)

## Funcionalidades Implementadas

- ✅ CRUD completo de usuários (User)
- ✅ Autenticação JWT com token bearer (email e senha)
- ✅ Upload de fotos de usuários para AWS S3 (URL retornada no objeto do usuário)
- ✅ URL da foto (S3) retornada no recurso do usuário para exibição/download pelo cliente
- ✅ Paginação de resultados
- ✅ Versionamento de API (v1)
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

Isso criará as tabelas no banco de dados.

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
| POST | `/api/v1/auth` | Autenticação (obter token JWT com email e senha) | Não |
| POST | `/api/v1/user` | Criar novo usuário | Sim |
| GET | `/api/v1/user` | Listar usuários (com paginação) | Sim |
| GET | `/api/v1/user/{id}` | Buscar usuário por ID | Sim |

> **Nota**: A API utiliza apenas a versão **v1** (`/api/v1/...`).

### Autenticação

O projeto utiliza autenticação JWT (JSON Web Token). Para acessar endpoints protegidos:

#### Credenciais de Teste

Cadastre um usuário via `POST /api/v1/user` (com email, senha, nome, data de nascimento e opcionalmente foto) e use o mesmo **email** e **senha** para obter o token.

- **Validade do Token**: 3 horas

#### Exemplo de Autenticação

**1. Obter o token (email e senha):**

```bash
curl -X POST "https://localhost:7198/api/v1/auth?email=seu@email.com&password=suasenha"
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
     "https://localhost:7198/api/v1/user?pageNumber=1&pageQuantity=10"
```

#### Autenticação no Swagger

1. Clique no botão **Authorize** no topo da página
2. Digite: `Bearer {seu-token-aqui}`
3. Clique em **Authorize**
4. Agora você pode testar os endpoints protegidos

### Exemplos de Requisições

#### Criar Usuário

```bash
curl -X POST "https://localhost:7198/api/v1/user" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: multipart/form-data" \
  -F "Name=João Silva" \
  -F "DateOfBirth=1990-05-15" \
  -F "Email=joao@email.com" \
  -F "Password=suasenha123" \
  -F "Photo=@/caminho/para/foto.jpg"
```

#### Listar Usuários

```bash
curl -X GET "https://localhost:7198/api/v1/user?pageNumber=1&pageQuantity=10" \
  -H "Authorization: Bearer {token}"
```

#### Buscar por ID

```bash
curl -X GET "https://localhost:7198/api/v1/user/1" \
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

### JWT (JwtSettings)

O JWT é configurado em **appsettings** (seção `JwtSettings` com chave `ApiSecret`). Em desenvolvimento, use **User Secrets** ou variáveis de ambiente para não commitar valores reais. Em produção, utilize Azure Key Vault, AWS Secrets Manager ou similar e nunca commite secrets no repositório.

### Configuração AWS S3 (Cloud)

As credenciais e o bucket de armazenamento de fotos são configurados na seção `Cloud` do appsettings:

```json
"JwtSettings": { "ApiSecret": "..." },
"Cloud": {
  "FileStorageBucketName": "...",
  "AccessKeyId": "...",
  "SecretAccessKey": "..."
}
```

- **FileStorageBucketName**: nome do bucket S3 onde as fotos são enviadas
- **AccessKeyId** e **SecretAccessKey**: credenciais AWS (recomenda-se IAM com permissões mínimas para o bucket)
- A região está fixa no código como `USEast1`; em produção considere externalizar (ex.: configuração ou variável de ambiente)

⚠️ **Segurança**: Não commite credenciais reais. Use variáveis de ambiente ou IAM roles (ex.: em EC2/ECS) em produção.

## Modelos de Dados

### User (Usuário)

A senha não é exposta na API (armazenada como hash no banco).

```csharp
{
    "id": 1,                        // int (Primary Key)
    "name": "João Silva",           // string (obrigatório)
    "dateOfBirth": "1990-05-15",    // DateTime
    "photo": "https://bucket.s3.region.amazonaws.com/profileImage-...",  // string (nullable) - URL da imagem no S3
    "email": "joao@email.com"       // string (obrigatório, único)
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
- **Upload de arquivos e armazenamento em nuvem (AWS S3)**
- **Configuração sensível** via appsettings/ambiente (JWT, credenciais AWS)
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
- [ ] Tornar região S3 configurável (appsettings ou variável de ambiente)
- [ ] Suportar múltiplos provedores de storage (interface/estratégia)

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
- Apenas a versão **v1** está em uso
- O versionamento é feito via URL: `/api/v1/...`

### Swagger
- Configurado com suporte a autenticação Bearer token
- Documentação separada por versão da API
- Disponível apenas em ambiente de desenvolvimento

### Storage de Arquivos (AWS S3)
- Fotos enviadas no cadastro são enviadas ao bucket configurado em `Cloud:FileStorageBucketName`
- O nome do arquivo segue o padrão do código (ex.: `profileImage-{email}-{name}`)
- A URL retornada é construída pelo `S3FileStorageService` no formato `https://{bucket}.s3.{region}.amazonaws.com/{key}` e armazenada no campo `photo` do usuário
- O cliente pode exibir ou baixar a imagem diretamente pela URL retornada

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
