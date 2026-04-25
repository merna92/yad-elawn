# Yad ElAwn Backend Presentation Script

## 1. Project overview
Yad ElAwn is a donations platform that connects donors, charities, and beneficiaries.
The backend is built with ASP.NET Core Web API and SQL Server.

## 2. How to open the project
1. Open Visual Studio.
2. Open this project file:
   `C:\Users\ThinkPad\OneDrive\Documents\New project\yad-elawn\backend\api\YadElAwn.Api\YadElAwn.Api.csproj`
3. Run the project with `F5`.
4. Open the browser at:
   `http://localhost:5000/`
5. Swagger UI opens on the root page because the route prefix is empty.

## 3. Swagger links
- Swagger UI: `http://localhost:5000/`
- Swagger JSON: `http://localhost:5000/swagger/v1/swagger.json`

## 4. Main architecture
The backend is organized into layers:
- Controllers
- Services
- Repositories
- DTOs
- Models
- DbContext

## 5. Full request cycle
1. The frontend or Swagger sends an HTTP request.
2. The controller receives the request.
3. The service applies validation and business rules.
4. The repository accesses the database.
5. The DbContext maps the C# classes to SQL tables.
6. SQL Server stores or returns the data.
7. The response goes back to the frontend.

## 6. Registration flow
- The user chooses a role: donor, charity, beneficiary, or admin.
- The registration endpoint checks whether the email already exists.
- The password is hashed with BCrypt before saving.
- A User record is created first.
- The matching role record is created after that.
- A transaction is used so the whole process succeeds or fails together.

### Registration endpoints
- `POST /api/registrations/donor`
- `POST /api/registrations/charity`
- `POST /api/registrations/beneficiary`
- `POST /api/registrations/admin`

## 7. Login flow
- The user sends email and password.
- The backend finds the user by email.
- The password is verified with BCrypt.
- If valid, a JWT token is created.
- The frontend stores the token and uses it for protected endpoints.

### Login endpoint
- `POST /api/auth/login`

## 8. Donation flow
- The donor sends a donation request.
- The backend validates that at least one donation type exists.
- A Donation record is created first.
- Then the subtype record is created:
  - Food
  - Clothes
  - Medicine
  - MedicalSupplies
- The process is wrapped inside a transaction.

### Donation endpoints
- `GET /api/donations`
- `GET /api/donations/{id}`
- `GET /api/donations/available`
- `POST /api/donations`
- `PATCH /api/donations/{id}/status`

## 9. Messages, notifications, and matches
- Messages are saved to the database and shown in the inbox.
- Notifications are created when important events happen.
- Matches connect donations with the appropriate charity or beneficiary.

### Related endpoints
- `GET /api/messages/inbox/{userId}`
- `POST /api/messages`
- `GET /api/notifications/{userId}`
- `POST /api/notifications`
- `GET /api/matches`
- `POST /api/matches`

## 10. Design patterns used
- MVC / Web API pattern
- Repository pattern
- Service layer pattern
- Dependency injection
- DTO pattern
- Transaction pattern

## 11. What each layer does
- Controllers: receive requests and return responses.
- Services: handle business rules and application logic.
- Repositories: handle data access.
- DTOs: define request and response shapes.
- Models: represent database entities.
- DbContext: connects the code to SQL Server.

## 12. Swagger explanation
Swagger is the API documentation and testing page.
It shows all endpoints, request bodies, and responses.
It also lets the frontend team test the API without writing frontend code first.

## 13. How to use Swagger
1. Open `http://localhost:5000/`.
2. Select the endpoint you want to test.
3. Click the endpoint to expand it.
4. Click `Try it out`.
5. Fill in the request body.
6. Click `Execute`.
7. Read the status code and the response body.

## 14. What to say in the presentation
The backend is built as a layered system so each part has one responsibility.
Controllers receive requests, services handle the logic, repositories handle data access, and DbContext connects everything to SQL Server.
JWT is used for authentication, BCrypt is used for password hashing, and transactions protect the data from partial saves.

## 15. Short presentation summary
The project starts from the UI, passes through the controller, service, repository, and DbContext layers, then reaches SQL Server, and finally returns a response to the frontend.
