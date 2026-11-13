# Employee RESTful API

A REST API built with **ASP.NET Core (.NET 6)** that serves employee data from the **AdventureWorks2017** SQL Server sample database as JSON. University project on web services; the SOAP version of the same idea is in the `employee-soap-service` repo.

## Tech stack

- C# / ASP.NET Core Web API (.NET 6)
- ADO.NET (`System.Data.SqlClient`), async queries
- Swagger / OpenAPI (Swashbuckle)
- SQL Server / SQL Express, AdventureWorks2017

## Endpoints

| Method | Route | Description | Responses |
|---|---|---|---|
| GET | `/api/employees?top=50` | List employees ordered by `BusinessEntityID`. `top` is clamped to 1-500 (default 50). | `200` |
| GET | `/api/employees/{nationalId}` | One employee by national ID number. | `200`, `404` |

### Example

```
GET /api/employees/295847284
```

```json
{
  "businessEntityId": 1,
  "nationalIdNumber": "295847284",
  "loginId": "adventure-works\\ken0",
  "organizationLevel": null,
  "jobTitle": "Chief Executive Officer",
  "birthDate": "1969-01-29T00:00:00",
  "maritalStatus": "S",
  "gender": "M",
  "hireDate": "2009-01-14T00:00:00"
}
```

(Values are illustrative; they come from your AdventureWorks copy.)

## Project structure

```
RESTFUL API/
├── Controllers/EmployeesController.cs   # endpoints + data access
├── Models/Employee.cs                   # response model
├── Program.cs                           # app setup, Swagger
├── appsettings.json                     # config (no secrets)
└── Properties/launchSettings.json
```

## Run

**Requirements:** [.NET 6 SDK](https://dotnet.microsoft.com/download) or newer, and SQL Server (or Express) with [AdventureWorks2017](https://learn.microsoft.com/sql/samples/adventureworks-install-configure) restored.

```bash
cd "RESTFUL API"
dotnet run
```

Then open `/swagger` (shown in the console output) to try the endpoints.

### Configuring the database

`appsettings.json` defaults to Windows authentication on `localhost\SQLEXPRESS`. For another server or a SQL login, **keep credentials out of git** by using user secrets:

```bash
dotnet user-secrets set "ConnectionStrings:AdventureWorks" "Data Source=...;Initial Catalog=AdventureWorks2017;User ID=...;Password=..."
```

or the environment variable `ConnectionStrings__AdventureWorks`.

## Security notes

- All queries are parameterized (no SQL injection).
- No credentials in the repository.
- Learning project: the API has no authentication and is read-only.

## Possible next steps

Create/update/delete endpoints, authentication (JWT), unit tests, Dapper or EF Core instead of raw ADO.NET.
