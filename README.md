        Author - Nay Oo Kyaw
        Email - nayookyaw.nok@gmail.com

# UserApiDemo – ASP.NET Core 9 Practice Project & Testing (Unit and Integration)

This repository is a **learning and reference project** for building a simple **ASP.NET Core 9 Web API** with:

- User **Create** and **Retrieve** APIs
- **Entity Framework Core** with **SQL Server**
- **Unit Testing** using **xUnit + Moq**
- **Integration Testing** using **WebApplicationFactory + Testcontainers**
- Clean project separation and best practices

---

## 0. Prerequisites (Windows)

Make sure the following are installed:

### Required

#### .NET SDK 9
Verify installation:
`dotnet --version`

UserApiDemo/
│
├── UserApi/                     # ASP.NET Core 9 Web API
│   ├── Controllers/             # API endpoints
│   ├── Data/                    # DbContext and EF Core configuration
│   ├── Models/                  # Entity models
│   ├── Program.cs               # Application entry point
│   ├── appsettings.json         # Configuration
│   └── UserApi.csproj
│
├── UserApi.UnitTests/           # Unit tests (xUnit + Moq)
│   ├── Services/                # Business logic tests
│   ├── Controllers/             # Controller tests (mocked)
│   └── UserApi.UnitTests.csproj
│
├── UserApi.IntegrationTests/    # Integration tests (real HTTP + DB)
│   ├── CustomWebApplicationFactory.cs
│   ├── UserApiTests.cs
│   └── UserApi.IntegrationTests.csproj
│
└── UserApiDemo.sln
