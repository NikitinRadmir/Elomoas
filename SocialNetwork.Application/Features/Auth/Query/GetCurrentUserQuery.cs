using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Elomoas.Application.Features.Auth.Query
{
    public record GetCurrentUserQuery() : IRequest<string>;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrentUserQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Task.FromResult(userId ?? string.Empty);
        }
    }
} 