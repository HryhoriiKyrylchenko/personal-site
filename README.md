# Forja Personal Website

Personal website and portfolio for Hryhorii Kyrylchenko — programmer’s portfolio, blog, and contact platform.

---

## Overview

This project is a multi-page responsive personal site built with:

- **Backend:** .NET 9 (C#), REST API, clean architecture, PostgreSQL database
- **Frontend:** React, responsive design, light/dark theme support, multi-language (EN, PL, UA, RU)
- **Features:**
  - Portfolio with project showcase
  - Blog section with articles
  - About page with skills, learning roadmap, and work experience
  - Contact form and social links
  - Admin panel for managing content (protected by AWS Cognito authentication)
  - SEO and SMM readiness
  - Localization with hybrid approach (static + database)
  - Unit tests for core backend logic

---

## Architecture

- Domain-Driven Design (DDD) + Clean Architecture pattern
- Projects organized by layers: Domain, Application, Infrastructure, Web, Frontend
- Uses MediatR for CQRS pattern in application layer
- PostgreSQL for persistent data
- Localization data partly stored in DB, partly static JSON files
- Authentication for admin panel via AWS Cognito

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js & npm](https://nodejs.org/)
- PostgreSQL database
- AWS account for Cognito (optional, for admin auth)

### Running Backend

1. Configure connection string in `appsettings.json`
2. Run EF Core migrations:
   ```bash
   dotnet ef database update
   ```
3. Run the backend API:
   ```bash
   dotnet run --project src/PersonalSite.Web
   ```
   
### Running Frontend

1. Navigate to frontend folder:
   ```bash
   cd src/forja-frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start dev server:
   ```bash
   npm start
   ```

### Testing

- Run unit tests from the solution root:
   ```bash
   dotnet test
   ```

### Localization

- Supports English, Polish, Ukrainian, and Russian with a hybrid localization approach:

    - UI strings in JSON resource files

    - Dynamic content translations stored in the database

### Admin Panel

- Protected by AWS Cognito authentication. Admin user created manually in AWS Cognito user pool. Only authorized users can manage portfolio projects, blog posts, and site content.