using Microsoft.EntityFrameworkCore;
using Serilog;
using Subscription.Core.Implementations;
using Subscription.Core.Interfaces;
using Subscription.EntityFrameworkCore;
using Subscription.Implementations;
using Subscription.Model.Exceptions;
using SubscriptionApi.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSerilog((provider, configuration) => configuration
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(provider)
    .Enrich.FromLogContext());

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<ISubscriptionManager, SubscriptionManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/subscribe", static async (SubscribeRequestModel model, ISubscriptionManager manager) =>
{
    try
    {
        var result = await manager.SubscribeAsync(model.UserId, model.Email);
        return Results.Json(result);
    }
    catch (SubscriptionBadDataException exc)
    {
        return Results.BadRequest(exc.Message);
    }
});

app.MapPost("/unsubscribe", static async (string userId, ISubscriptionManager manager) =>
{
    try
    {
        await manager.DisableSubscriptionAsync(userId);
        return Results.Ok();
    }
    catch (SubscriptionBadDataException exc)
    {
        return Results.BadRequest(exc.Message);
    }
});

app.MapGet("/active", static async (ISubscriptionManager manager) =>
{
    var result = await manager.GetActiveSubscriptionsAsync();
    return Results.Json(result);
});

app.Run();
