# 📝 Blog Personal Backend

RESTful API developed with ASP.NET Core 8 for a personal blog platform.

## 🚀 Technologies

- **ASP.NET Core 8** — Web API framework
- **Entity Framework Core 8** — ORM
- **MySQL** — Database
- **JWT Bearer** — Authentication
- **BCrypt** — Password hashing
- **Swagger/OpenAPI** — Documentation
- **OpenAI** — AI integration
- **xUnit + Moq** — Unit testing
- **SonarCloud** — Code quality

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Docker](https://www.docker.com/) (for MySQL)
- [Git](https://git-scm.com/)

## ⚙️ Setup

### 1. Clone the repository
```bash
git clone https://github.com/OSIELJ/blog-personal-backend.git
cd blog-personal-backend
```

### 2. Start MySQL with Docker
```bash
docker run -d --name mysql-blog \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=db_blog_pessoal \
  -p 3306:3306 \
  mysql:8.0
```

### 3. Configure environment
Create `BlogPersonal/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=db_blog_pessoal;User=root;Password=root;"
  },
  "Jwt": {
    "Key": "BlogPessoalSecretKey2024!SuaChaveLongaAqui@123"
  }
}
```

### 4. Run migrations
```bash
cd BlogPersonal
dotnet ef database update
```

### 5. Run the application
```bash
dotnet run
```

Access Swagger at: **http://localhost:5204**

## 🔐 Authentication

### Register
```
POST /api/users/register
```
```json
{
  "name": "Your Name",
  "email": "your@email.com",
  "password": "yourpassword"
}
```

### Login
```
POST /api/users/login
```
```json
{
  "email": "your@email.com",
  "password": "yourpassword"
}
```
Returns a JWT token. Use it in the **Authorize** button on Swagger:
```
Bearer {your_token_here}
```

## 📌 Endpoints

### Users
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | /api/users | ✅ | List all users |
| GET | /api/users/{id} | ✅ | Get user by id |
| POST | /api/users/register | ❌ | Register user |
| POST | /api/users/login | ❌ | Login |
| PUT | /api/users/{id} | ✅ | Update user |
| DELETE | /api/users/{id} | ✅ | Delete user |

### Themes
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | /api/themes | ✅ | List all themes |
| GET | /api/themes/{id} | ✅ | Get theme by id |
| GET | /api/themes/description/{desc} | ✅ | Search by description |
| POST | /api/themes | ✅ | Create theme |
| PUT | /api/themes/{id} | ✅ | Update theme |
| DELETE | /api/themes/{id} | ✅ | Delete theme |

### Posts
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | /api/posts | ✅ | List all posts |
| GET | /api/posts/{id} | ✅ | Get post by id |
| GET | /api/posts/title/{title} | ✅ | Search by title |
| GET | /api/posts/filter | ✅ | Filter by user/theme |
| POST | /api/posts | ✅ | Create post |
| PUT | /api/posts/{id} | ✅ | Update post |
| DELETE | /api/posts/{id} | ✅ | Delete post |

### AI
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/ai/summarize | ✅ | Generate AI summary |

## 🧪 Tests

```bash
cd BlogPersonal.Tests
dotnet test
```

- **100 unit tests**
- **84.7% code coverage**

## 📊 Code Quality

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OSIELJ_blog-personal-backend&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OSIELJ_blog-personal-backend)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=OSIELJ_blog-personal-backend&metric=coverage)](https://sonarcloud.io/summary/new_code?id=OSIELJ_blog-personal-backend)

## 📁 Project Structure

```
blog-personal-backend/
├── BlogPersonal/
│   ├── Config/          # JWT configuration
│   ├── Controllers/     # API controllers
│   ├── Data/            # DbContext
│   ├── DTOs/            # Data transfer objects
│   ├── Migrations/      # EF migrations
│   ├── Models/          # Domain models
│   ├── Repositories/    # Data access layer
│   ├── Services/        # Business logic
│   │   └── IA/          # AI integration
│   └── Program.cs
└── BlogPersonal.Tests/  # Unit tests
```

## 👤 Author

**Osiel Junior**
- GitHub: [@OSIELJ](https://github.com/OSIELJ)