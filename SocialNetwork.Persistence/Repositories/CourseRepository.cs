using Microsoft.EntityFrameworkCore;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elomoas.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly IGenericRepository<Course> _repository;

    public CourseRepository(IGenericRepository<Course> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _repository.Entities.ToListAsync();
    }

    public async Task<Course> GetCourseById(int id)
    {
        return await _repository.Entities.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Dictionary<string, int>> GetCoursesCountByPL()
    {
        return await _repository.Entities
            .GroupBy(x => x.PL)
            .ToDictionaryAsync(
                g => g.Key.ToString(),
                g => g.Count()
            );
    }
}
