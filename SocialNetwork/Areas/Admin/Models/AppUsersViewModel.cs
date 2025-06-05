using Elomoas.Application.Features.AppUsers.Query;

namespace SocialNetwork.Areas.Admin.Models
{
    public class AppUsersViewModel
    {
        public IEnumerable<AppUserDto> Users { get; set; }
    }
} 