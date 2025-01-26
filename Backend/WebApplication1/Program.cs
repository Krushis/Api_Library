using LibraryBackend;
using LibraryBackend.Data;
using LibraryBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<FileCleanupService>();

builder.Services.AddControllers();

var app = builder.Build();

// for HTTP request pipeline
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
