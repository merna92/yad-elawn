# Yad ElAwn - Full Project

Organized repository for the Yad ElAwn donation platform.

## Structure
- `frontend/` - Frontend app (placeholder)
- `backend/` - Backend services
  - `backend/api/YadElAwn.Api/` - ASP.NET Core Web API
- `ai/` - AI components (placeholder)
- `database/` - SQL Server schema and scripts
- `docs/` - Documentation (idea, overview)

## Backend Quick Start
1. Open the API project:
   `backend/api/YadElAwn.Api/YadElAwn.Api.csproj`
2. Update connection string in:
   `backend/api/YadElAwn.Api/appsettings.json`
3. Run the API (F5 in Visual Studio).
4. Swagger UI:
   `http://localhost:5000/`

## Auth
1. Register:
   `POST /api/registrations/donor`
2. Login:
   `POST /api/auth/login`
3. Use token:
   `Authorization: Bearer <TOKEN>`
