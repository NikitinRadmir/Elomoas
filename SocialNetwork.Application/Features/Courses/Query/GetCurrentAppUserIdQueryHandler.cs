using MediatR;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Application.Features.Courses.Query;

public class GetCurrentAppUserIdQueryHandler : IRequestHandler<GetCurrentAppUserIdQuery, int?>
{
    private readonly IAppUserRepository _userRepository;
    public GetCurrentAppUserIdQueryHandler(IAppUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<int?> Handle(GetCurrentAppUserIdQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync();
        var user = users.FirstOrDefault(u => u.IdentityId == request.IdentityId);
        return user?.Id;
    }
} 