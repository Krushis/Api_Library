using LibraryBackend;
using LibraryBackend.Data;
using LibraryBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Because frontend had difficulties connecting to backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// adds the logger to the builder
builder.Logging.AddConsole();

// add in-memory database
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseInMemoryDatabase("LibraryDatabase"));

// registers bookService that has business logic like calculating the price of a book when you want to order it and
// sets up methods on the Db to use
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<UserService>();

// swagger services, doesnt work anymore for testing, since it does not handle picture uploading, unless you change it a bit
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// JWT Authentication setup
var jwtKey = builder.Configuration["Jwt:Key"];  // Fetch the JWT key from the appsettings.json
var jwtIssuer = builder.Configuration["Jwt:Issuer"]; // For issuer verification
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Validate that the token's issuer matches the expected issuer
        ValidateAudience = true,  // Validate that the token's audience matches the expected audience
        ValidateLifetime = true,  // Ensure that the token has not expired
        ValidateIssuerSigningKey = true,  // Ensure the token's signing key is valid (i.e., it’s signed with the expected key)
        ValidIssuer = jwtIssuer, // If validating the issuer, this is the expected value
        ValidAudience = jwtIssuer, // The audience claim is often the same as the issuer, can be different though
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // The secret key used for signing and validating the token
    };
});


// THis is for cleaning up wwroot/images folder after we stop running the program (since in-memory db)
builder.Services.AddHostedService<FileCleanupService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

    // If no users exist, add one
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            UserName = "testuser",
            PassWord = "test123", // Use your hash helper
            Role = "User"
        });
        db.SaveChanges();
    }
}

// Add authentication middleware
app.UseAuthentication(); // Make sure to add this middleware before authorization
app.UseAuthorization(); // Authorization middleware

// for HTTP request pipeline
app.UseCors("AllowAll");
app.UseStaticFiles();
//app.UseSwagger();
//app.UseSwaggerUI();
app.MapControllers();

app.Run();
