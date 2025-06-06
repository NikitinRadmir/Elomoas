using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Features.Messenger.Dto;

using Microsoft.EntityFrameworkCore;
using Elomoas.Application.Interfaces.Repositories;
using System.Linq;

namespace Elomoas.Application.Features.Messenger.Queries.GetUserChats
{
    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, List<UserChatDto>>
    {
        private readonly IChatService _chatService;
        private readonly IAppUserRepository _userRepository;

        public GetUserChatsQueryHandler(IChatService chatService, IAppUserRepository userRepository)
        {
            _chatService = chatService;
            _userRepository = userRepository;
        }

        public async Task<List<UserChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _chatService.GetUserChatsAsync(request.UserId);
            var users = (await _userRepository.GetAllUsersAsync()).ToList();
            var result = new List<UserChatDto>();
            foreach (var chat in chats)
            {
                var otherUserId = chat.User1Id == request.UserId ? chat.User2Id : chat.User1Id;
                var otherUser = users.FirstOrDefault(u => u.IdentityId == otherUserId);
                var lastMessage = chat.Messages.OrderByDescending(m => m.CreatedDate).FirstOrDefault();
                result.Add(new UserChatDto
                {
                    ChatId = chat.Id,
                    UserId = otherUserId,
                    UserName = otherUser?.Name ?? "Unknown",
                    UserEmail = otherUser?.Email ?? "Unknown",
                    UserImage = otherUser?.Img ?? "/images/default-icon.jpg",
                    LastMessage = lastMessage != null ? (lastMessage.Content.Length > 30 ? lastMessage.Content.Substring(0, 27) + "..." : lastMessage.Content) : "",
                    LastMessageTime = lastMessage?.CreatedDate,
                    UnreadCount = chat.Messages.Count(m => !m.IsRead && m.SenderId == otherUserId)
                });
            }
            return result.OrderByDescending(c => c.LastMessageTime).ToList();
        }
    }
} 