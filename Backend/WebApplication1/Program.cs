using LibraryBackend;

var builder = WebApplication.CreateBuilder(args);

// Add services from Startup
var startup = new Startup();
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.MapControllers();

app.Run();
