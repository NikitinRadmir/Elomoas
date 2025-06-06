using MediatR;
using Elomoas.Application.Features.Settings.Queries.GetAccountInfo;

namespace Elomoas.Application.Features.Settings.Queries.GetAccountInfo
{
    public class GetAccountInfoQuery : IRequest<GetAccountInfoDto>
    {
        public string IdentityId { get; set; }

        public GetAccountInfoQuery(string identityId)
        {
            IdentityId = identityId;
        }
    }
} 