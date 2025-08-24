using Toolidol.Api.Services;
using Toolidol.Api.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient factory
builder.Services.AddHttpClient();

// App services
builder.Services.AddScoped<HttpService>();
builder.Services.AddScoped<ILinkedInOrganizationService, LinkedInOrganizationService>();
builder.Services.AddScoped<ILinkedInAnalyticsService, LinkedInAnalyticsService>();

// Options binding
builder.Services.Configure<LinkedInOptions>(builder.Configuration.GetSection("LinkedIn"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
