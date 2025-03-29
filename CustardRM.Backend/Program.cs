using CustardRM.Common.Interfaces;
using CustardRM.Common.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

builder.Services.AddRateLimiter(options =>
{
	options.AddPolicy("fixed", context =>
		RateLimitPartition.GetFixedWindowLimiter(
			partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
			factory: key => new FixedWindowRateLimiterOptions
			{
				PermitLimit = 5,
				Window = TimeSpan.FromSeconds(10),
				QueueLimit = 0,
				QueueProcessingOrder = QueueProcessingOrder.OldestFirst
			}));

	options.OnRejected = async (context, token) =>
	{
		context.HttpContext.Response.StatusCode = 429;
		await context.HttpContext.Response.WriteAsync("Rate limit exceeded.", token);
	};
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRateLimiter();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
