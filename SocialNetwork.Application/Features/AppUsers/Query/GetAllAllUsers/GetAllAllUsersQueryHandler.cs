using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Application.Interfaces.Repositories;
using MediatR;


namespace SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers
{
    public class GetAllAllUsersQueryHandler : IRequestHandler<GetAllAllUsersQuery, IEnumerable<AppUserDto>>
    {
        private readonly IAppUserRepository _userRepository;

        public GetAllAllUsersQueryHandler(IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<AppUserDto>> Handle(GetAllAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new AppUserDto
            {
                Id = user.Id,
                IdentityId = user.IdentityId,
                Name = user.Name,
                Email = user.Email,
                Img = user.Img ?? "/images/default-icon.jpg",
                Description = user.Description,
                Password = user.Password
            });
        }
    }
} 