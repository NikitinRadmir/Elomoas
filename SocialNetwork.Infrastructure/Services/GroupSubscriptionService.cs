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
            return await _context.GroupSubscriptions
                .Include(s => s.User)
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving group subscription with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> CreateSubscriptionAsync(GroupSubscription subscription)
    {
        try
        {
            _logger.LogInformation("Creating subscription for user {UserId} to group {GroupId}", 
                subscription.UserId, subscription.GroupId);

            subscription.CreatedDate = DateTime.UtcNow;
            _context.GroupSubscriptions.Add(subscription);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation("Successfully created subscription for user {UserId} to group {GroupId}", 
                    subscription.UserId, subscription.GroupId);
                return true;
            }

            _logger.LogWarning("Failed to create subscription for user {UserId} to group {GroupId}", 
                subscription.UserId, subscription.GroupId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription for user {UserId} to group {GroupId}", 
                subscription.UserId, subscription.GroupId);
            throw;
        }
    }

    public async Task<bool> CreateSubscriptionAsync(int userId, int groupId)
    {
        var subscription = new GroupSubscription
        {
            UserId = userId,
            GroupId = groupId,
            CreatedDate = DateTime.UtcNow
        };

        return await CreateSubscriptionAsync(subscription);
    }

    public async Task<bool> UpdateSubscriptionAsync(GroupSubscription subscription)
    {
        try
        {
            _logger.LogInformation("Attempting to update subscription {Id}", subscription.Id);

            var existingSubscription = await _context.GroupSubscriptions.FindAsync(subscription.Id);
            if (existingSubscription == null)
            {
                _logger.LogWarning("Subscription {Id} not found for update", subscription.Id);
                return false;
            }

            existingSubscription.UserId = subscription.UserId;
            existingSubscription.GroupId = subscription.GroupId;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation("Successfully updated subscription {Id}", subscription.Id);
                return true;
            }

            _logger.LogWarning("Failed to update subscription {Id}", subscription.Id);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}", subscription.Id);
            throw;
        }
    }

    public async Task<bool> UpdateSubscriptionAsync(int id, int userId, int groupId)
    {
        var subscription = new GroupSubscription
        {
            Id = id,
            UserId = userId,
            GroupId = groupId,
        };

        return await UpdateSubscriptionAsync(subscription);
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