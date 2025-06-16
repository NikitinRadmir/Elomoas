using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using System.Linq;
using Elomoas.Application.Features.Courses.Query;

namespace Elomoas.Infrastructure.Repositories
{
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

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _repository.Entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddCourseAsync(string name, string description, string img, decimal price, ProgramLanguage pl, string video, string learn)
        {
            var course = new Course
            {
                Name = name,
                Description = description,
                Img = string.IsNullOrEmpty(img) ? "/images/v-1.png" : img,
                Price = price,
                PL = pl,
                Video = string.IsNullOrEmpty(video) ? "/images/video4.mp4" : video,
                Learn = learn
            };
            await _repository.AddAsync(course);
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await GetCourseByIdAsync(id);
            if (course == null) return false;
            await _repository.DeleteAsync(course);
            return true;
        }

        public async Task<bool> UpdateCourseAsync(int id, string name, string description, string img, decimal price, ProgramLanguage pl, string video, string learn)
        {
            var existingCourse = await GetCourseByIdAsync(id);
            if (existingCourse == null) return false;

            existingCourse.Name = name ?? existingCourse.Name;
            existingCourse.Description = description ?? existingCourse.Description;
            existingCourse.Img = img ?? existingCourse.Img;
            existingCourse.Price = price;
            existingCourse.PL = pl;
            existingCourse.Video = video ?? existingCourse.Video;
            existingCourse.Learn = learn ?? existingCourse.Learn;

            await _repository.UpdateAsync(existingCourse);
            return true;
        }

        public async Task<Dictionary<string, int>> GetCoursesCountByPL()
        {
            var courses = await _repository.Entities.ToListAsync();
            return courses
                .GroupBy(x => x.PL)
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Count()
                );
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsDto()
        {
            var courses = await _repository.Entities.ToListAsync();
            return courses.Select(x => new CourseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Img = x.Img,
                Price = x.Price,
                PL = x.PL,
                Video = x.Video,
                Learn = x.Learn,
                IsCurrentUserSubscribed = false
            });
        }
    }
} 