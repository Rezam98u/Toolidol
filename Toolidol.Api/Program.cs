using Toolidol.Api.Services;
using Toolidol.Api.Options;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Toolidol.Api.Services.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient factory
// Resilient LinkedIn HttpClient
builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Services.AddHttpClient("LinkedInClient")
	.AddHttpMessageHandler<LoggingDelegatingHandler>()
	.AddPolicyHandler(HttpPolicyExtensions
		.HandleTransientHttpError()
		.OrResult(msg => (int)msg.StatusCode == 429)
		.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
	.AddPolicyHandler(HttpPolicyExtensions
		.HandleTransientHttpError()
		.CircuitBreakerAsync(5, TimeSpan.FromMinutes(1)));

// App services
builder.Services.AddScoped<HttpService>();
builder.Services.AddScoped<ILinkedInOrganizationService, LinkedInOrganizationService>();
builder.Services.AddScoped<ILinkedInAnalyticsService, LinkedInAnalyticsService>();

// Options binding
builder.Services.Configure<LinkedInOptions>(builder.Configuration.GetSection("LinkedIn"));

// In-memory cache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
