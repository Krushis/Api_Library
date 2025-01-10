using LibraryBackend.Data;
using LibraryBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Allows to setup different domain things
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Logger to log important information
builder.Logging.AddConsole();

// in-memory database
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseInMemoryDatabase("LibraryDatabase"));

builder.Services.AddTransient<BookService>();

builder.Services.AddEndpointsApiExplorer(); // Swagger needs it, can I move it to Startup?
builder.Services.AddSwaggerGen();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowAll"); // applies to http request pipeline
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
// Map controllers 
app.MapControllers();

app.Run();

