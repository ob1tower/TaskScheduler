using Microsoft.OpenApi.Models;
using NoteService.API.Extensions;
using NoteService.API.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddLogging();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddSingleton<RabbitMqService>(sp =>
{
    var hostName = builder.Configuration.GetValue<string>("RabbitMq:HostName");
    var logger = sp.GetRequiredService<ILogger<RabbitMqService>>();
    return new RabbitMqService(hostName, logger);
});

builder.Services.AddScoped<UserRegisterService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var rabbitMqService = app.Services.GetRequiredService<RabbitMqService>();

rabbitMqService.ListenForMessages(async message =>
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var userRegisterService = services.GetRequiredService<UserRegisterService>();
        var logger = services.GetRequiredService<ILogger<RabbitMqService>>();
        await userRegisterService.ProcessMessageAsync(message);
    }
});

app.Run();
