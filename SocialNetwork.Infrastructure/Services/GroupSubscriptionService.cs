using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Infrastructure.Services;

public class GroupSubscriptionService : IGroupSubscriptionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GroupSubscriptionService> _logger;

    public GroupSubscriptionService(
        ApplicationDbContext context,
        ILogger<GroupSubscriptionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GroupSubscription>> GetAllGroupSubscriptionsAsync()
    {
        try
        {
            return await _context.GroupSubscriptions
                .Include(s => s.User)
                .Include(s => s.Group)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all group subscriptions");
            throw;
        }
    }

    public async Task<GroupSubscription?> GetSubscriptionByIdAsync(int id)
    {
        try
        {
            return await _context.GroupSubscriptions.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving group subscription with id {Id}", id);
            throw;
        }
    }

    public async Task<GroupSubscription> CreateSubscriptionAsync(GroupSubscription subscription)
    {
        try
        {
            subscription.CreatedDate = DateTime.UtcNow;
            _context.GroupSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new group subscription with id {Id}", subscription.Id);
            return subscription;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating group subscription");
            throw;
        }
    }

    public async Task<bool> UpdateSubscriptionAsync(GroupSubscription subscription)
    {
        try
        {
            _logger.LogInformation("Attempting to update subscription {Id}", subscription.Id);
            _context.GroupSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated subscription {Id}", subscription.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}", subscription.Id);
            throw;
        }
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        try
        {
            var subscription = await _context.GroupSubscriptions.FindAsync(id);
            if (subscription == null)
            {
                _logger.LogWarning("Group subscription {Id} not found for deletion", id);
                return false;
            }

            _context.GroupSubscriptions.Remove(subscription);
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