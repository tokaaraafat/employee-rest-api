using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using RESTFUL_API.Models;

namespace RESTFUL_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private const string Columns =
        "BusinessEntityID, NationalIDNumber, LoginID, OrganizationLevel, " +
        "JobTitle, BirthDate, MaritalStatus, Gender, HireDate";

    private readonly string _connectionString;

    public EmployeesController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AdventureWorks")
            ?? throw new InvalidOperationException(
                "Connection string 'AdventureWorks' is not configured. See the README.");
    }

    /// <summary>Returns the first <paramref name="top"/> employees (1-500, default 50).</summary>
    // GET api/employees?top=20
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAll([FromQuery] int top = 50)
    {
        top = Math.Clamp(top, 1, 500);

        const string sql = "SELECT TOP (@top) " + Columns +
                           " FROM HumanResources.Employee ORDER BY BusinessEntityID";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@top", SqlDbType.Int).Value = top;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        var employees = new List<Employee>();
        while (await reader.ReadAsync())
        {
            employees.Add(Map(reader));
        }
        return Ok(employees);
    }

    /// <summary>Returns one employee by national ID number, or 404.</summary>
    // GET api/employees/295847284
    [HttpGet("{nationalId}")]
    public async Task<ActionResult<Employee>> GetByNationalId(string nationalId)
    {
        const string sql = "SELECT " + Columns +
                           " FROM HumanResources.Employee WHERE NationalIDNumber = @nationalId";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@nationalId", SqlDbType.NVarChar, 15).Value = nationalId;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return NotFound();
        }
        return Ok(Map(reader));
    }

    private static Employee Map(SqlDataReader r) => new(
        r.GetInt32(0),
        r.GetString(1),
        r.GetString(2),
        r.IsDBNull(3) ? null : r.GetInt16(3),
        r.GetString(4),
        r.GetDateTime(5),
        r.GetString(6),
        r.GetString(7),
        r.GetDateTime(8));
}
