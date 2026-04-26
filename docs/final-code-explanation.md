# Final Backend Presentation Guide

## 1. Project idea
Yad ElAwn is a donations platform that connects donors, charities, and beneficiaries.
The backend was built with ASP.NET Core Web API and SQL Server.

## 2. Database idea
The database was created first so the backend could match real tables and relationships.
The main entities are:
- User
- Donor
- Charity
- Beneficiary
- Admin
- Donation
- Food
- Clothes
- Medicine
- MedicalSupplies
- Message
- Notification
- Match
- Location

## 3. Backend structure
The project is organized into layers:
- Controllers
- Services
- Repositories
- DTOs
- Models
- DbContext

## 4. File explanation

### Program.cs
This is the startup file.
It registers services, configures Swagger, enables JWT authentication, and connects the project to SQL Server.

### ApplicationDbContext.cs
This file maps the C# classes to the database tables.
It also defines the relationships between entities.

### Models/Entities.cs
This file contains the entity classes that represent the database tables.

### Dtos/Requests.cs
This file contains request and response models.
It keeps the API contract separate from the database entities.

### Controllers
Controllers receive HTTP requests and return HTTP responses.
They do not contain the main business logic.

### Services
Services contain the business rules.
Examples:
- AuthService
- RegistrationService
- DonationService

### Repositories
Repositories handle database access.
Examples:
- UserRepository
- DonationRepository

## 5. Full request cycle
1. The user or Swagger sends a request.
2. The controller receives the request.
3. The service checks validation and business rules.
4. The repository handles the query or insert.
5. The DbContext maps the data to SQL tables.
6. SQL Server stores or returns the data.
7. The response goes back to the frontend.

## 6. Registration flow
1. The user chooses a role.
2. The frontend sends the registration request.
3. The backend checks if the email already exists.
4. BCrypt hashes the password.
5. The User record is created.
6. The role record is created.
7. A transaction makes the process safe.
8. The backend returns the created IDs.

## 7. Login flow
1. The user sends email and password.
2. The backend finds the user by email.
3. BCrypt verifies the password.
4. If valid, JWT is created.
5. The backend returns the token.
6. The frontend uses the token in protected endpoints.

## 8. Donation flow
1. The donor sends a donation request.
2. The backend validates the data.
3. A Donation record is created.
4. The subtype record is created.
5. The whole operation runs inside a transaction.
6. The backend returns the created donation.

## 9. Messages, notifications, and matches
- Messages are saved in the database and can be shown in the inbox.
- Notifications are created when important events happen.
- Matches connect donations with charities or beneficiaries.

## 10. Design patterns used
- MVC / Web API pattern
- Repository pattern
- Service layer pattern
- Dependency injection
- DTO pattern
- Transaction pattern

## 11. Swagger usage
Open Swagger UI here:
http://localhost:5000/

Swagger JSON:
http://localhost:5000/swagger/v1/swagger.json

### How to test an endpoint
1. Open the endpoint section.
2. Click Try it out.
3. Fill in the request body.
4. Click Execute.
5. Read the response code and body.

## 12. What to say in the presentation
The backend follows a layered architecture.
Controllers receive requests, services apply business logic, repositories access the database, and DbContext connects the code to SQL Server.
JWT protects the login process, BCrypt protects passwords, and transactions protect multi-step operations.

## 13. Short presentation summary
This backend starts from the frontend, passes through controller, service, repository, and DbContext layers, then reaches SQL Server, and finally returns a response.
