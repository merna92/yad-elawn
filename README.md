# Yad ElAwn - Backend API

This repository contains the backend API and database scripts for the "Yad ElAwn" donation platform.

## Structure
- `backend/YadElAwn.Api/` - ASP.NET Core Web API project
- `database/` - SQL Server schema and scripts
- `database/erd/` - ERD diagrams
- `docs/` - Project documentation

## Quick Start
1. Open the API project:
   `backend/YadElAwn.Api/YadElAwn.Api.csproj`
2. Update connection string in:
   `backend/YadElAwn.Api/appsettings.json`
3. Run the API (F5 in Visual Studio).
4. Swagger UI:
   `http://localhost:5000/`

## Authentication
1. Register a user:
   `POST /api/registrations/donor`
2. Login:
   `POST /api/auth/login`
3. Use token:
   `Authorization: Bearer <TOKEN>`

## Notes
- Passwords are hashed using BCrypt.
- Swagger UI is served at the root `/` for easy access.
