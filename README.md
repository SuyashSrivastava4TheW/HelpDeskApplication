# HelpDesk Management System

A full-stack Help Desk Management System built using **ASP.NET Core 8**, following a clean layered architecture with separate API and MVC projects. The application allows users to create, manage, update, and track support tickets efficiently.

---

## 🚀 Features

- **Create Support Tickets**: Submit new help desk tickets (status defaults to `Open`).
- **View & Track Tickets**: View all submitted tickets with real-time dashboard status counters.
- **Update Tickets**: Modify ticket details, priority (`Low`, `Medium`, `High`), and status (`Open`, `In Progress`, `Closed`).
- **Delete Tickets**: Remove existing tickets with standard confirmation.
- **Status Filtering**: Filter tickets by status (`Open`, `In Progress`, `Closed`).
- **RESTful Web API**: Decoupled Web API backend with Swagger OpenAPI documentation.
- **ASP.NET Core MVC Frontend**: Interactive Razor views with responsive Bootstrap design.
- **Entity Framework Core**: Supports SQLite (zero-config local persistence) and SQL Server LocalDB.
- **Global Exception Middleware**: Structured problem detail responses for HTTP 400, 404, and 500 errors.
- **Automated Database Setup**: Automatic database creation (`EnsureCreated()`) on application startup.
- **Unit Testing Support**: xUnit testing project included.

---

## 🛠️ Tech Stack

### Backend
- **ASP.NET Core 8 Web API**
- **Entity Framework Core 8** (SQLite / SQL Server)
- **AutoMapper**
- **Serilog** (Logging to Console and Files)
- **C# 12**

### Frontend
- **ASP.NET Core MVC**
- **Razor Views**
- **Bootstrap 5**

### Testing
- **xUnit**

---

## 📁 Project Structure

```
HelpDeskManagement
│
├── HelpDesk.Api
│   ├── Controllers         # REST API Endpoints (TicketController)
│   ├── DTOs                # Data Transfer Objects (Create, Update, Read)
│   ├── Data                # AppDbContext (EF Core Code-First)
│   ├── Exceptions          # Custom Exception Classes (NotFoundException, BadRequestException)
│   ├── Mapping             # AutoMapper Profiles (TicketMappingProfile)
│   ├── Middleware          # Global Exception Handling Middleware
│   ├── Models              # Domain Entities & Constants (Ticket, TicketConstants)
│   ├── Repositories        # Repository Pattern (ITicketRepository, TicketRepository)
│   ├── Services            # Business Logic Layer (ITicketService, TicketService)
│   ├── Program.cs          # API Startup Configuration & Dependency Injection
│   └── appsettings.json    # Database Connection Strings & App Settings
│
├── HelpDesk.Mvc
│   ├── Controllers         # MVC Controllers (HomeController, TicketController)
│   ├── Models              # View Models & DTOs
│   ├── Services            # Typed HttpClient Service (TicketApiService)
│   ├── Views               # Razor Views (Dashboard, Ticket Views)
│   ├── wwwroot             # Static Assets (CSS, JS, Bootstrap)
│   └── Program.cs          # MVC Application Startup
│
├── HelpDesk.Tests          # Unit Tests
│
└── HelpDeskManagement.sln  # Visual Studio Solution File
```

---

## 🔌 API Endpoints

| Method | Endpoint | Description |
|:---|:---|:---|
| `GET` | `/api/Ticket/All` | Get all tickets (ordered by newest first) |
| `GET` | `/api/Ticket/{id}` | Get a single ticket by ID |
| `POST` | `/api/Ticket` | Create a new ticket (Status defaults to `Open`) |
| `PUT` | `/api/Ticket/{id}` | Update ticket details, priority, and status |
| `DELETE` | `/api/Ticket/{id}` | Delete a ticket by ID |
| `GET` | `/api/Ticket/Status/{status}` | Filter tickets by status (`Open`, `In Progress`, `Closed`) |

---

## ⚙️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/SuyashSrivastava4TheW/HelpDeskManagment.git
cd HelpDeskManagement
```

### 2. Restore Dependencies & Build Solution

```bash
dotnet restore
dotnet build HelpDeskManagement.sln
```

### 3. Run the Backend API

Open a terminal window and run:

```bash
cd HelpDesk.Api
dotnet run
```

- **API Base URL**: `https://localhost:59186`
- **Swagger Documentation**: `https://localhost:59186/swagger` (or root `https://localhost:59186/`)

---

### 4. Run the MVC Frontend Application

Open a second terminal window and run:

```bash
cd HelpDesk.Mvc
dotnet run
```

- **MVC Frontend Web App**: `https://localhost:59188`

---

## 📖 Swagger API Documentation

Once the API project is running, open your browser and navigate to:

```
https://localhost:59186/swagger
```

You can interactively test all REST API endpoints, inspect request/response DTO schemas, and view real-time API responses.

---