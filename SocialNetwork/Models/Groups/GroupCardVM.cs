using Elomoas.Domain.Entities.Enum;

namespace Elomoas.mvc.Models.Groups
{
    public class GroupCardVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public ProgramLanguage PL { get; set; }
        public bool IsCurrentUserSubscribed { get; set; }
    }
} 