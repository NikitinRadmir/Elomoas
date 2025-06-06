using Elomoas.Application.Features.Groups.Query.GetAll;
using Elomoas.Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Areas.Admin.Models;

public class GroupsViewModel
{
    public IEnumerable<GetAllDto> Groups { get; set; }
}

public class CreateGroupViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    public string Img { get; set; }

    [Required(ErrorMessage = "Programming Language is required")]
    public ProgramLanguage PL { get; set; }
}

public class UpdateGroupViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    public string Img { get; set; }

    [Required(ErrorMessage = "Programming Language is required")]
    public ProgramLanguage PL { get; set; }
} 