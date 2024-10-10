using Artemis.Data;
using Microsoft.EntityFrameworkCore;

namespace Artemis.API.Services
{
    public class DatabaseMigrationService
    {
        private readonly ApplicationDBContext _context;

        public DatabaseMigrationService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task MigrateAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await _context.Database.MigrateAsync();
        }
    }
}
