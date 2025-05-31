using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Domain.Entities.Enum;

public enum ProgramLanguage
{
    [Display(Name = "BOOTSTRAP")]
    BOOTSTRAP,
    [Display(Name = "HTML")]
    HTML,
    [Display(Name = "JQUERY")]
    JQUERY,
    [Display(Name = "SASS")]
    SASS,
    [Display(Name = "REACT")]
    REACT,
    [Display(Name = "JAVA")]
    JAVA,
    [Display(Name = "PYTHON")]
    PYTHON,
    [Display(Name = "MONGODB")]
    MONGODB
}
