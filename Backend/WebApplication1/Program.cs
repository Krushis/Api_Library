using LibraryBackend;
using LibraryBackend.Data;
using LibraryBackend.Services;
using Microsoft.EntityFrameworkCore;

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

// THis is for cleaning up wwroot/images folder after we stop running the program (since in-memory db)
builder.Services.AddHostedService<FileCleanupService>();

builder.Services.AddControllers();

var app = builder.Build();

// for HTTP request pipeline
app.UseCors("AllowAll");
app.UseStaticFiles();
//app.UseSwagger();
//app.UseSwaggerUI();
app.MapControllers();

app.Run();
