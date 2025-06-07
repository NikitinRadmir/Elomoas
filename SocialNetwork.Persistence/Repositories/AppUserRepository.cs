using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;

namespace Elomoas.Persistence.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByIdentityIdAsync(string identityId)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityId);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _context.AppUsers
                .OrderBy(u => u.Id)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByIdWithIdentityAsync(int id)
        {
            return await _context.AppUsers
                .Include(x => x.IdentityUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            return await _context.AppUsers.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersByIdentityIdsAsync(IEnumerable<string> identityIds)
        {
            return await _context.AppUsers
                .Where(u => identityIds.Contains(u.IdentityId))
                .ToListAsync();
        }

        public async Task<AppUser> UpdateAsync(AppUser user)
        {
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
