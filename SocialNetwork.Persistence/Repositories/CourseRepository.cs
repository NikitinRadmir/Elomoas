using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Persistence.Repositories
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

        public async Task<Course> GetCourseById(int id)
        {
            return await _repository.Entities.FirstOrDefaultAsync(x => x.Id == id);
        }


        //Task<IEnumerable<Course>> GetAllCoursesAsync ();
        //Task<Course> GetCourseById(int id);
    }
}
