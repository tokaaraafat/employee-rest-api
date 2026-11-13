namespace RESTFUL_API.Models;

public record Employee(
    int BusinessEntityId,
    string NationalIdNumber,
    string LoginId,
    short? OrganizationLevel,
    string JobTitle,
    DateTime BirthDate,
    string MaritalStatus,
    string Gender,
    DateTime HireDate);
