using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elomoas.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private const string DEFAULT_IMAGE = "/uploads/profiles/default-icon.jpg";

    public UserService(
        ApplicationDbContext context, 
        UserManager<IdentityUser> userManager,
        ILogger<UserService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Find AppUser first
            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Id == id);

            if (appUser == null)
            {
                _logger.LogWarning("AppUser with id {Id} not found", id);
                return false;
            }

            _logger.LogInformation("Found AppUser with id {Id} and IdentityId {IdentityId}", id, appUser.IdentityId);

            // Delete all friendships where user is either friend1 or friend2
            var friendships = await _context.Friendships
                .Where(f => f.UserId == appUser.IdentityId || f.FriendId == appUser.IdentityId)
                .ToListAsync();

            if (friendships.Any())
            {
                _context.Friendships.RemoveRange(friendships);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted {Count} friendships for user {Id}", friendships.Count, id);
            }

            // Delete AppUser
            _context.AppUsers.Remove(appUser);
            await _context.SaveChangesAsync();

            // Then find and delete IdentityUser if exists
            if (!string.IsNullOrEmpty(appUser.IdentityId))
            {
                var identityUser = await _userManager.FindByIdAsync(appUser.IdentityId);
                if (identityUser != null)
                {
                    var identityResult = await _userManager.DeleteAsync(identityUser);
                    if (!identityResult.Succeeded)
                    {
                        _logger.LogError("Failed to delete IdentityUser {IdentityId}: {Errors}", 
                            appUser.IdentityId, 
                            string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
                else
                {
                    _logger.LogWarning("IdentityUser with id {IdentityId} not found", appUser.IdentityId);
                }
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Successfully deleted user with id {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {Id}", id);
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(AppUser user)
    {
        if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
            return false;

        var existingUser = await _context.AppUsers.FindAsync(user.Id);
        if (existingUser == null)
            return false;

        // Check if email is changed and if new email already exists
        if (existingUser.Email != user.Email)
        {
            var emailExists = await _context.AppUsers
                .AnyAsync(u => u.Email == user.Email && u.Id != user.Id);
            if (emailExists)
                return false;
        }

        // Update only allowed fields
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Description = user.Description;
        
        // Set default image if empty
        existingUser.Img = string.IsNullOrEmpty(user.Img) ? DEFAULT_IMAGE : user.Img;

        _context.AppUsers.Update(existingUser);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
} 