using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;

namespace Elomoas.Persistence.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly IGenericRepository<AppUser> _repository;

        public AppUserRepository(IGenericRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _repository.Entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByIdWithIdentityAsync(int id)
        {
            return await _repository.Entities
                .Include(x => x.IdentityUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            return await _repository.Entities.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersByIdentityIdsAsync(IEnumerable<string> identityIds)
        {
            return await _repository.Entities
                .Where(u => identityIds.Contains(u.IdentityId))
                .ToListAsync();
        }
    }
}
