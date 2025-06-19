using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.Application.Interfaces.Services;

public interface IGroupService
{
    Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
    Task<GroupDto?> GetGroupByIdAsync(int id);
    Task<Group?> GetGroupEntityByIdAsync(int id);
    Task<bool> CreateGroupAsync(string name, string description, string img, ProgramLanguage pl);
    Task<bool> UpdateGroupAsync(Group group);
    Task<bool> DeleteGroupAsync(int id);
} 