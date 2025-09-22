using DemoApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder
            .WithOrigins("*") // 👈 origen de tu Angular
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Inyectar SqlHelper
builder.Services.AddSingleton<SqlHelper>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAngularDev");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();

/*
 using DemoApi.Data;
using DemoApi.Models;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Inyectar SqlHelper
builder.Services.AddSingleton<SqlHelper>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Endpoints Students

app.MapGet("/api/students", async (SqlHelper sqlHelper) =>
{
    var dt = await sqlHelper.ExecuteStoredProcedureAsync("sp_GetAllStudents");
    var students = dt.AsEnumerable().Select(row => new Student
    {
        Id = row.Field<int>("Id"),
        Name = row.Field<string>("Name"),
        Age = row.Field<int>("Age"),
        Email = row.Field<string>("Email")
    }).ToList();

    return Results.Ok(students);
});

app.MapGet("/api/students/{id}", async (int id, SqlHelper sqlHelper) =>
{
    var dt = await sqlHelper.ExecuteStoredProcedureAsync("sp_GetStudentById",
        new SqlParameter("@Id", id));

    if (dt.Rows.Count == 0) return Results.NotFound();

    var row = dt.Rows[0];
    var student = new Student
    {
        Id = (int)row["Id"],
        Name = row["Name"].ToString()!,
        Age = (int)row["Age"],
        Email = row["Email"].ToString()!
    };

    return Results.Ok(student);
});

app.MapPost("/api/students", async (Student student, SqlHelper sqlHelper) =>
{
    await sqlHelper.ExecuteNonQueryAsync("sp_CreateStudent",
        new SqlParameter("@Name", student.Name),
        new SqlParameter("@Age", student.Age),
        new SqlParameter("@Email", student.Email));

    return Results.Ok(new { message = "Student created successfully" });
});

app.MapPut("/api/students/{id}", async (int id, Student student, SqlHelper sqlHelper) =>
{
    await sqlHelper.ExecuteNonQueryAsync("sp_UpdateStudent",
        new SqlParameter("@Id", id),
        new SqlParameter("@Name", student.Name),
        new SqlParameter("@Age", student.Age),
        new SqlParameter("@Email", student.Email));

    return Results.Ok(new { message = "Student updated successfully" });
});

app.MapDelete("/api/students/{id}", async (int id, SqlHelper sqlHelper) =>
{
    await sqlHelper.ExecuteNonQueryAsync("sp_DeleteStudent",
        new SqlParameter("@Id", id));

    return Results.Ok(new { message = "Student deleted successfully" });
});

#endregion

app.Run();

 */
