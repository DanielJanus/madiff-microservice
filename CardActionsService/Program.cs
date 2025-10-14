using System.Text.Json;
using CardActionsService.Services;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CardService>();
builder.Services.AddSingleton<AllowedCardActionsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var result = JsonSerializer.Serialize(new { error = exception?.Message ?? "Internal Server Error" });
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(result);
    });
});


app.UseHttpsRedirection();

app.MapControllers();

app.Run();