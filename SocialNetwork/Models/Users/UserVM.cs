using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Domain.Entities;
using System.Collections.Generic;

namespace Elomoas.mvc.Models.Users
{
    public class UserVM
    {
        public IEnumerable<AppUserDto> Users { get; set; }
        public AppUserDto User { get; set; }
        public IEnumerable<Group> SubscribedGroups { get; set; }
    }
}

