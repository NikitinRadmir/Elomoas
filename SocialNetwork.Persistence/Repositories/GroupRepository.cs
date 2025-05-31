using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = Elomoas.Domain.Entities.Group;

namespace Elomoas.Persistence.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IGenericRepository<Group> _repository;

        public GroupRepository(IGenericRepository<Group> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _repository.Entities.ToListAsync();
        }
    }
}
