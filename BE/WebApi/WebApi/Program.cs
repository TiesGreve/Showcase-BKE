using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using WebApi.Data;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder => {
                          builder
                            .WithOrigins("http://localhost:5501") // specifying the allowed origin
                            .WithOrigins("https://showcaseclient.pages.dev")
                            .WithMethods("POST") // defining the allowed HTTP method
                            .WithMethods("GET")
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/Acceslog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

app.Run();
