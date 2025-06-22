using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using StockManagement.Endpoints;
using StockManagement.Middleware;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(opt =>
{
    opt.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Stock Management Demo API";
        document.Info.Contact = new OpenApiContact()
        {
            Name = "Engin",
            Email = "Engin@test.com"
        };
        return Task.CompletedTask;
    });
});

//Enable Cross-Origin requests from the localhost client app
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173");
                      });
});

// Add rate limiting configuration
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 60; // Maximum 60 requests
        limiterOptions.Window = TimeSpan.FromSeconds(30); // Per 30 seconds
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0; // No queuing allowed
    });
});

//Cross-Site Request Forgery protection
builder.Services.AddAntiforgery(options =>
{
    // Set Cookie properties using CookieBuilder properties†.
    options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
    options.SuppressXFrameOptionsHeader = false;
});

var fakeStoreUrl = builder.Configuration["FakeStoreUrl"];

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();
app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<AntiXssMiddleware>();
app.UseExceptionHandler();

app.MapProductsEndpoints(fakeStoreUrl!);
app.MapOrdersEndpoints(fakeStoreUrl!);

app.Run();
