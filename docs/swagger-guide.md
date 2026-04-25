# Swagger Quick Guide

## Open Swagger
Use this URL in your browser:
`http://localhost:5000/`

## What you will see
- A list of all endpoints.
- The request body for each POST/PATCH endpoint.
- The response examples.

## How to test an endpoint
1. Open the endpoint section.
2. Click `Try it out`.
3. Fill in the request body.
4. Click `Execute`.
5. Check the response code and body.

## Important login step
After a successful login, copy the JWT token and use it in protected endpoints:
`Authorization: Bearer <TOKEN>`

## Helpful URLs
- Swagger UI: `http://localhost:5000/`
- Swagger JSON: `http://localhost:5000/swagger/v1/swagger.json`
