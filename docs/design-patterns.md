# Design Patterns Used in Yad ElAwn

This backend uses a layered architecture so the code is easier to maintain, explain, and extend.

## 1) MVC / Web API Pattern
- Controllers receive the HTTP requests.
- Services contain the business logic.
- Repositories handle database access.

## 2) Repository Pattern
- `UserRepository` hides the database logic for users.
- `DonationRepository` hides the database logic for donations.
- Controllers do not talk directly to EF Core for these main flows.

## 3) Service Layer Pattern
- `AuthService` handles login and token creation.
- `RegistrationService` handles all user registration rules.
- `DonationService` handles donation creation and status updates.

## 4) Dependency Injection
- Services and repositories are injected into controllers.
- This reduces coupling and makes testing easier.

## 5) DTO Pattern
- Requests like `LoginRequest`, `RegisterDonorRequest`, and `CreateDonationRequest` are separated from database entities.
- This keeps the API contract clean.

## 6) Transaction Pattern
- Registration and donation creation use database transactions.
- If one step fails, everything is rolled back.

## 7) Controlled Creation Flow
- Donation creation decides which subtype entity to create based on the request body.
- This keeps creation logic in one place and makes future extension easier.

## What to say in class
> The backend follows a layered architecture with MVC, Repository, and Service patterns.  
> Controllers are thin, business logic lives in services, and database access is abstracted in repositories.  
> I also used Dependency Injection, DTOs, and transactions to keep the code clean and scalable.
