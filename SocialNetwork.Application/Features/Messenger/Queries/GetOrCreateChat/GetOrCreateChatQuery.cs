using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Queries.GetOrCreateChat;

public record GetOrCreateChatQuery(string UserId, string FriendId) : IRequest<ChatDto>; 