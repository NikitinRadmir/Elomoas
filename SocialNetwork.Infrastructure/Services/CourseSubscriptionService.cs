using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elomoas.Infrastructure.Services;

public class CourseSubscriptionService : ICourseSubscriptionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CourseSubscriptionService> _logger;

    public CourseSubscriptionService(
        ApplicationDbContext context,
        ILogger<CourseSubscriptionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<CourseSubscription>> GetAllCourseSubscriptionsAsync()
    {
        
            try
            {
                return await _context.CourseSubscriptions.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all courses");
                throw;
            }
        
    }

    public async Task<CourseSubscription?> GetSubscriptionByIdAsync(int id)
    {
        try
        {
            return await _context.CourseSubscriptions.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course with id {Id}", id);
            throw;
        }
    }

    public async Task<CourseSubscription> CreateSubscriptionAsync(CourseSubscription subscription)
    {
        try
        {
            subscription.CreatedDate = DateTime.UtcNow;
            _context.CourseSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new course with id {Id}", subscription.Id);
            return subscription;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course");
            throw;
        }
    }

    public async Task<bool> UpdateSubscriptionAsync(CourseSubscription subscription)
    {
        try
        {
            _logger.LogInformation("Attempting to update course {Id}", subscription.Id);

            // Ensure the course exists and get creation info
            var existingCourse = await _context.CourseSubscriptions.FindAsync(subscription.Id);
            if (existingCourse == null)
            {
                _logger.LogWarning("Course {Id} not found for update", subscription.Id);
                return false;
            }

            // Preserve creation info
            subscription.CreatedBy = existingCourse.CreatedBy;
            subscription.CreatedDate = existingCourse.CreatedDate;

            // Use Update method
            _context.CourseSubscriptions.Update(subscription);

            // Save changes
            var result = await _context.SaveChangesAsync();

            _logger.LogInformation("Update affected {Count} records for course {Id}", result, subscription.Id);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating course {subscription.Id}", subscription.Id);
            throw;
        }
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        try
        {
            var subscription = await _context.CourseSubscriptions.FindAsync(id);
            if (subscription == null)
            {
                _logger.LogWarning($"Subscription {id} not found for deletion", id);
                return false;
            }

            _context.CourseSubscriptions.Remove(subscription);
            var result = await _context.SaveChangesAsync();
            
            _logger.LogInformation("Deletion affected {Count} records for subscription {Id}", result, id);
            
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription {Id}", id);
            throw;
        }
    }
} 