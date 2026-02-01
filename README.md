# 🌐 Personal Website & Portfolio

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

> ⚠️ Completed initial MVP implementation. Project is functional and production ready at baseline level.

A modern, multi-language personal website and portfolio with a blog, contact form, project showcase, and secure admin panel.

---

## 🚀 Overview

This full-stack site is built to showcase development projects, publish articles, and enable professional contact. It includes:

- 🖥 **Backend**: .NET 9 (C#), REST API, PostgreSQL, Domain-Driven Design, Clean Architecture, Vertical Slice Architecture
- 🌐 **Frontend**: Angular, responsive layout, light/dark themes, i18n (EN, PL, UA, RU)
- 🔐 **Admin Panel**: Authentication for content management
- 🌍 **Localization**: Hybrid static and dynamic translation system
- 🧪 **Testing**: Unit tests for core business logic

---

## ✨ Features

- 💼 Portfolio with project showcases
- ✍️ Blog section with articles
- 👤 About page with skills, roadmap, and experience
- 📬 Contact form with email notifications
- 🔗 Social media links
- ⚙️ Admin panel for content management
- 🈳 Localization with JSON + database translations
- 🧪 Unit testing with xUnit, Moq, and FluentAssertions

---

## 🧱 Architecture

- **Architecture**: Clean Architecture + Domain-Driven Design (DDD) + Vertical Slice Architecture
- **Structure**:
    - Each feature is self-contained (vertical slices): commands, queries, handlers, validators, mappings
    - Core layers:
        - `Domain`: Business entities and rules
        - `Application`: CQRS with MediatR, use cases
        - `Infrastructure`: Persistence, email, external services
        - `Web`: API endpoints (controllers)
        - `Frontend`: Angular UI
- **Data**: PostgreSQL, Entity Framework Core (Code First)
- **Localization**:
    - UI text in static JSON files
    - Content translations in database

---

## 🛠 Getting Started

### ✅ Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js & npm](https://nodejs.org/)
- PostgreSQL
- AWS account (for S3 storage) or MinIO (as S3 compatible service)

---

### 🧩 Backend Setup

1. Configure the database connection string using one of the following:

    - **Local development**: Use [`.NET user-secrets`](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to store sensitive values securely:

      ```bash
      dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=...;Username=...;Password=..."
      ```

    - **Production**: Store secrets in a secure store such as **AWS Systems Manager Parameter Store** or **AWS Secrets Manager** and load them at runtime.

2. Apply EF Core migrations:

    ```bash
    dotnet ef database update
    ```

3. Run the API:

    ```bash
    dotnet run --project src/PersonalSite.Web
    ```

---

### 🎨 Frontend Setup

1. Public Site:

    ```bash
    cd src/frontend/public-site
    npm install
    npm start
    ```

2. Admin panel:

    ```bash
    cd src/frontend/admin-panel
    npm install
    npm start
    ```

---

### 🧪 Running Tests

- Run unit tests from the solution root:
   ```bash
   dotnet test
   ```
  
---

### 🌍 Localization

The application supports the following languages:
- English (EN)
- Polish (PL)
- Ukrainian (UA)
- Russian (RU)

**Localization Details:**
- *Static UI strings* are stored in separate JSON files per language.
- *Dynamic content* translations are maintained in the database.

---

### 🔐 Admin Panel

The admin interface is secured using cookie-based authentication.

**Access Control:**
- The admin user account is seeded and requires an initial password change.
- Only **authenticated users** can manage:
    - Portfolio projects
    - Blog posts
    - Page content and translations

---

## 📦 Technologies Used

**Backend:**
- .NET 9
- C#
- Entity Framework Core (EF Core)
- MediatR
- PostgreSQL
- MinIO (optional)

**Frontend:**
- Angular
- SCSS

**Cloud:**
- AWS S3 

**Tooling:**
- xUnit
- Moq
- FluentAssertions
- GitHub Actions

---

## 📄 License

This project is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for full details.
