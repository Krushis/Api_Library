using LibraryBackend.Data;
using LibraryBackend.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>(opt => opt.UseInMemoryDatabase("LibraryDb"));
            services.AddScoped<BookService>();
            services.AddControllers();
        }
    }
}
