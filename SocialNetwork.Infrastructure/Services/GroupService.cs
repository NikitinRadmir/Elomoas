using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Features.Groups.Query.GetAll;
using System.Linq;

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

    public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
    {
        try
        {
            var groups = await _context.Groups.ToListAsync();
            return groups.Select(group => new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Img = group.Img ?? "/images/default-icon.jpg",
                PL = group.PL,
                IsCurrentUserSubscribed = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all groups");
            throw;
        }
    }

    public async Task<GroupDto?> GetGroupByIdAsync(int id)
    {
        try
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return null;
            }

            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Img = group.Img ?? "/images/default-icon.jpg",
                PL = group.PL,
                IsCurrentUserSubscribed = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving group with id {Id}", id);
            throw;
        }
    }

    public async Task<Group?> GetGroupEntityByIdAsync(int id)
    {
        try
        {
            return await _context.Groups.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving group entity with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> CreateGroupAsync(string name, string description, string img, ProgramLanguage pl)
    {
        try
        {
            var group = new Group
            {
                Name = name,
                Description = description,
                Img = img,
                PL = pl,
                CreatedDate = DateTime.UtcNow
            };

            _context.Groups.Add(group);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                _logger.LogInformation("Created new group with id {Id}", group.Id);
                return true;
            }

            _logger.LogWarning("Failed to create group with name {Name}", name);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating group with name {Name}", name);
            throw;
        }
    }

    public async Task<bool> UpdateGroupAsync(Group group)
    {
        try
        {
            _logger.LogInformation("Attempting to update group {Id}", group.Id);

            // Ensure the group exists and get creation info
            var existingGroup = await _context.Groups.FindAsync(group.Id);
            if (existingGroup == null)
            {
                _logger.LogWarning("Group {Id} not found for update", group.Id);
                return false;
            }

            // Preserve creation info
            group.CreatedBy = existingGroup.CreatedBy;
            group.CreatedDate = existingGroup.CreatedDate;

            // Use Update method
            _context.Groups.Update(group);

            // Save changes
            var result = await _context.SaveChangesAsync();
            
            _logger.LogInformation("Update affected {Count} records for group {Id}", result, group.Id);
            
            return result > 0;
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
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                _logger.LogInformation("Successfully deleted group {Id}", id);
                return true;
            }

            _logger.LogWarning("Failed to delete group {Id}", id);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting group {Id}", id);
            throw;
        }
    }
} 