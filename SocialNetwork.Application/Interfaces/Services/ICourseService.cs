using Elomoas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services;

public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task<Course> CreateCourseAsync(Course course);
    Task<Course> UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(int id);
} 