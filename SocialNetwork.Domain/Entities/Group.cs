using Elomoas.Domain.Common;
using Elomoas.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Domain.Entities;

public class Group : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Img { get; set; }
    public ProgramLanguage PL { get; set; }
}
