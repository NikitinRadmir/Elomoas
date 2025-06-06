using System.Collections.Generic;
using System.Threading.Tasks;
using Elomoas.Application.Features.Messenger.Dto;

namespace Elomoas.Application.Interfaces.Services;

public interface IUserChatService
{
    Task<List<UserChatDto>> GetUserChatsAsync(string userId);
} 