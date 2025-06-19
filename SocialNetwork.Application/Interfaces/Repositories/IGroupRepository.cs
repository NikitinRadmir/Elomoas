using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group?> GetByIdAsync(int id);
    }
}
