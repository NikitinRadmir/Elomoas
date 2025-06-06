using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Elomoas.Persistence.Repositories
{
    public class SettingsRepository
    {
        private readonly ApplicationDbContext _context;
        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Settings>> GetAllAsync()
            => await _context.Settings.ToListAsync();

        public async Task<Settings?> GetByIdAsync(int id)
            => await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Settings?> GetByUserIdAsync(string userId)
            => await _context.Settings.FirstOrDefaultAsync(s => s.UserId == userId);

        public async Task<Settings> CreateAsync(Settings settings)
        {
            _context.Settings.Add(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<bool> UpdateAsync(Settings settings)
        {
            var existing = await _context.Settings.FindAsync(settings.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(settings);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var settings = await _context.Settings.FindAsync(id);
            if (settings == null) return false;
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 