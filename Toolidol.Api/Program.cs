using Toolidol.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient factory
builder.Services.AddHttpClient();

// App services
builder.Services.AddScoped<HttpService>();
builder.Services.AddScoped<ILinkedInOrganizationService, LinkedInOrganizationService>();
builder.Services.AddScoped<ILinkedInAnalyticsService, LinkedInAnalyticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
