using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using To_do_list.Data;
using To_do_list.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPoliticaCors", policy =>
    {
        policy.SetIsOriginAllowed(origin => origin.
                StartsWith("*"))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Content-Disposition");
    });

    options.AddPolicy("MiPoliticaCorsS", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
);

var app = builder.Build();

app.UseExceptionHandler(exceptionApp => exceptionApp.Run(async context => {
    context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var status = context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            AggregateException => StatusCodes.Status400BadRequest,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        context.Response.StatusCode = status;
        
        if (exception is FluentValidation.ValidationException validationEx)
        {
            var errors = validationEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            var response = new ResponseException(
                status,
                "Errores de validación",
                errors);

            await context.Response.WriteAsJsonAsync(response);
            return;
        }
        
        var generic = new ResponseException(
            status,
            exception?.Message ?? "Error interno",
            new Dictionary<string, string[]>()
            );

        await context.Response.WriteAsJsonAsync(generic);
}));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MiPoliticaCorsS"); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();