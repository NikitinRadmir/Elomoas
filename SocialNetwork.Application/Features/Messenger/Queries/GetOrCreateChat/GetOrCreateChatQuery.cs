using MediatR;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Messenger.Queries.GetOrCreateChat;

public record GetOrCreateChatQuery(string UserId, string FriendId) : IRequest<Chat>; 