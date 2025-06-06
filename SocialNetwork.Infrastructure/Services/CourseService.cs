using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

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
            throw;
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
            throw;
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

    public async Task<bool> UpdateCourseAsync(Course course)
    {
        try
        {
            _logger.LogInformation("Attempting to update course {Id}", course.Id);

            // Ensure the course exists and get creation info
            var existingCourse = await _context.Courses.FindAsync(course.Id);
            if (existingCourse == null)
            {
                _logger.LogWarning("Course {Id} not found for update", course.Id);
                return false;
            }

            // Preserve creation info
            course.CreatedBy = existingCourse.CreatedBy;
            course.CreatedDate = existingCourse.CreatedDate;

            // Use Update method
            _context.Courses.Update(course);

            // Save changes
            var result = await _context.SaveChangesAsync();
            
            _logger.LogInformation("Update affected {Count} records for course {Id}", result, course.Id);
            
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course {Id}", course.Id);
            throw;
        }
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                _logger.LogWarning("Course {Id} not found for deletion", id);
                return false;
            }

            _context.Courses.Remove(course);
            var result = await _context.SaveChangesAsync();
            
            _logger.LogInformation("Deletion affected {Count} records for course {Id}", result, id);
            
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course {Id}", id);
            throw;
        }
    }
} 