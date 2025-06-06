using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elomoas.Infrastructure.Services;

public class GroupService : IGroupService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GroupService> _logger;

    public GroupService(
        ApplicationDbContext context,
        ILogger<GroupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Group>> GetAllGroupsAsync()
    {
        try
        {
            return await _context.Groups.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all groups");
            throw;
        }
    }

    public async Task<Group?> GetGroupByIdAsync(int id)
    {
        try
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving group with id {Id}", id);
            throw;
        }
    }

    public async Task<Group> CreateGroupAsync(Group group)
    {
        try
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Created new group with id {Id}", group.Id);
            return group;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating group");
            throw;
        }
    }

    public async Task<bool> UpdateGroupAsync(Group group)
    {
        try
        {
            _logger.LogInformation("Attempting to update group {Id}", group.Id);

            var existingGroup = await _context.Groups
                .FirstOrDefaultAsync(g => g.Id == group.Id);

            if (existingGroup == null)
            {
                _logger.LogWarning("Group {Id} not found", group.Id);
                return false;
            }

            // Update properties
            group.CreatedBy = existingGroup.CreatedBy;
            group.CreatedDate = existingGroup.CreatedDate;

            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Successfully updated group {Id}", group.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating group {Id}", group.Id);
            throw;
        }
    }

    public async Task<bool> DeleteGroupAsync(int id)
    {
        try
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                _logger.LogWarning("Group {Id} not found for deletion", id);
                return false;
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Successfully deleted group {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting group {Id}", id);
            throw;
        }
    }
} 