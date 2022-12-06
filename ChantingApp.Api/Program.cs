using System.Reflection;
using ChantingApp.Api;
using ChantingApp.Api.Services;
using ChantingApp.Api.Validators;
using ChantingApp.Api.ViewModels;
using ChantingApp.DbMigrations;
using ChantingApp.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLogging();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddMediatR(Assembly.GetEntryAssembly()!);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ChantsDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("ChantsDbConnection"),
                   sqlOptions => { sqlOptions.MigrationsAssembly(typeof(MigrationsAssemblyMarker).Assembly.FullName); });
});
builder.Services.AddTransient<IValidator<CreateChantViewModel>, CreateChantValidator>();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger().UseSwaggerUI();
app.UseAuthentication()
   .UseRouting()
   .UseAuthorization()
   .UseEndpoints(e => { e.MapControllers(); });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChantsDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();