using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elomoas.Application.Features.AppUsers.Query.GetAllUsers;
using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Features.AppUsers.Query;

namespace Elomoas.Application.Features.AppUsers.Query.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AppUserDto>
    {
        private readonly IAppUserRepository _userRepository;

        public GetUserByIdQueryHandler(IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AppUserDto> Handle(GetUserByIdQuery query, CancellationToken ct)
        {
            var data = await _userRepository.GetUserByIdAsync(query.id);
            var result = new AppUserDto
            {
                IdentityId = data.IdentityId,
                Name = data.Name,
                Description = data.Description,
                Img = data.Img,
                Email = data.Email,
                Password = data.Password,

            };

            return result ?? new AppUserDto() { };
        }
    }
}
