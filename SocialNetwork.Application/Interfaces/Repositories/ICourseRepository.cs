using System.Collections.Generic;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Application.Features.Courses.Query;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<bool> AddCourseAsync(string name, string description, string img, decimal price, ProgramLanguage pl, string video, string learn);
        Task<bool> DeleteCourseAsync(int id);
        Task<bool> UpdateCourseAsync(int id, string name, string description, string img, decimal price, ProgramLanguage pl, string video, string learn);
        Task<Dictionary<string, int>> GetCoursesCountByPL();
        Task<IEnumerable<CourseDto>> GetAllCoursesAsDto();
    }
}
