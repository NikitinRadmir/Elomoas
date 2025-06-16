using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.Friends.Dtos;

namespace Elomoas.Application.Features.Friends.Queries.GetAllFriendships;

public record GetAllFriendshipsQuery : IRequest<IEnumerable<FriendshipDto>>; 