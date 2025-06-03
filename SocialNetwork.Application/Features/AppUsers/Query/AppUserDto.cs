using Elomoas.Domain.Common;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.AppUsers.Query;

public class AppUserDto
{
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Img { get; set; }
    public string Description { get; set; }
    public string Password { get; set; }
    public FriendshipStatus? FriendshipStatus { get; set; }
    public bool IsFriend { get; set; }
    public bool IsSentByMe { get; set; }
}