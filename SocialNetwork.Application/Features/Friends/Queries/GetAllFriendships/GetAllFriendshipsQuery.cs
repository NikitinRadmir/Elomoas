using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Friends.Queries.GetAllFriendships;

public record GetAllFriendshipsQuery : IRequest<IEnumerable<Friendship>>; 