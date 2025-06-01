using Elomoas.Application.Features.AppUsers.Query;

namespace Elomoas.mvc.Models.Users
{
    public class UserVM
    {
        public IEnumerable<AppUserDto> Users { get; set; }
        public AppUserDto User { get; set; }
    }
}

