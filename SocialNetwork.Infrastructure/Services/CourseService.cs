using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elomoas.Infrastructure.Services;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CourseService> _logger;

    public CourseService(
        ApplicationDbContext context,
        ILogger<CourseService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        try
        {
            return await _context.Courses.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all courses");
            return Enumerable.Empty<Course>();
        }
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        try
        {
            return await _context.Courses.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course with id {Id}", id);
            return null;
        }
    }

    public async Task<Course> CreateCourseAsync(Course course)
    {
        try
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new course with id {Id}", course.Id);
            return course;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course");
            throw;
        }
    }

    public async Task<Course> UpdateCourseAsync(Course course)
    {
        try
        {
            var existingCourse = await _context.Courses.FindAsync(course.Id);
            if (existingCourse == null)
            {
                _logger.LogWarning("Course with id {Id} not found for update", course.Id);
                throw new KeyNotFoundException($"Course with id {course.Id} not found");
            }

            // Update only allowed fields
            existingCourse.Name = course.Name;
            existingCourse.Description = course.Description;
            existingCourse.Img = course.Img;
            existingCourse.Price = course.Price;
            existingCourse.PL = course.PL;
            existingCourse.Video = course.Video;
            existingCourse.Learn = course.Learn;

            _context.Courses.Update(existingCourse);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Updated course with id {Id}", course.Id);
            return existingCourse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course with id {Id}", course.Id);
            throw;
        }
    }

    public async Task DeleteCourseAsync(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                _logger.LogWarning("Course with id {Id} not found for deletion", id);
                return;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted course with id {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course with id {Id}", id);
            throw;
        }
    }
} 