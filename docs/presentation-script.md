# Yad ElAwn Backend Presentation Script

## 1. Project idea
Yad ElAwn is a donations platform that connects donors, charities, and beneficiaries.
The backend is built with ASP.NET Core Web API and SQL Server.

## 2. Main architecture
The backend is organized into layers:
- Controllers
- Services
- Repositories
- DTOs
- Models
- DbContext

## 3. Full request cycle
1. The frontend or Swagger sends an HTTP request.
2. The controller receives the request.
3. The service checks the business rules.
4. The repository accesses the database.
5. The DbContext maps the classes to the SQL tables.
6. SQL Server stores or returns the data.
7. The response goes back to the frontend.

## 4. Registration flow
- The user chooses a role: donor, charity, beneficiary, or admin.
- The registration endpoint checks whether the email already exists.
- The password is hashed with BCrypt before saving.
- A User record is created first.
- The matching role record is created after that.
- A transaction is used so the whole process succeeds or fails together.

## 5. Login flow
- The user sends email and password.
- The backend finds the user by email.
- The password is verified with BCrypt.
- If valid, a JWT token is created.
- The frontend stores the token and uses it for protected endpoints.

## 6. Donation flow
- The donor sends a donation request.
- The backend validates that at least one donation type exists.
- A Donation record is created first.
- Then the subtype record is created: Food, Clothes, Medicine, or MedicalSupplies.
- The process is wrapped inside a transaction.

## 7. Messages, notifications, and matches
- Messages are saved to the database and shown in the inbox.
- Notifications are created when important events happen.
- Matches connect donations with the appropriate charity or beneficiary.

## 8. Design patterns used
- MVC / Web API pattern
- Repository pattern
- Service layer pattern
- Dependency injection
- DTO pattern
- Transaction pattern

## 9. Swagger
Swagger is the API documentation and testing page.
Open it in the browser using:
http://localhost:5000/

## 10. What to say in the presentation
The backend is built as a layered system so each part has one responsibility.
Controllers receive requests, services handle the logic, repositories handle data access, and DbContext connects everything to SQL Server.
JWT is used for authentication, BCrypt is used for password hashing, and transactions protect the data from partial saves.
