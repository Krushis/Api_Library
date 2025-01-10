using LibraryBackend.Data;
using LibraryBackend.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddDbContext<LibraryContext>(opt => opt.UseInMemoryDatabase("LibraryDb"));
            services.AddTransient<BookService>(); // this is depenedency injection
            services.AddScoped<BookService>();
            services.AddControllers();
        }
    }
}
