using Elomoas.Domain.Common;
using Elomoas.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Domain.Entities;

public class AppUser : BaseAuditableEntity
{
    public string IdentityId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Img { get; set; }
    public string? Description { get; set; }
    public string? Password { get; set; }
    public int Id { get; set; }
    public string? ProfileImage { get; set; }
    public virtual IdentityUser IdentityUser { get; set; }
}