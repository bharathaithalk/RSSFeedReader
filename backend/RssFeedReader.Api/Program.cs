using RssFeedReader.Api.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemorySubscriptionStore>();
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    options.AddPolicy("LocalUi", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("LocalUi");
}

app.MapGet("/api/subscriptions", (InMemorySubscriptionStore store) => Results.Ok(store.GetAll()));

app.MapPost("/api/subscriptions", (AddSubscriptionRequest? request, InMemorySubscriptionStore store) =>
{
    if (request is null || string.IsNullOrWhiteSpace(request.Url))
    {
        return Results.BadRequest(new { title = "Invalid subscription URL", status = 400 });
    }

    var url = request.Url;
    var existing = store.GetAll().FirstOrDefault(item => string.Equals(item.Url, url, StringComparison.Ordinal));
    if (existing is not null)
    {
        return Results.Ok(existing);
    }

    var created = store.Add(url);
    return Results.Created("/api/subscriptions", created);
});

app.Run();

public record AddSubscriptionRequest(string? Url);

public partial class Program { }
