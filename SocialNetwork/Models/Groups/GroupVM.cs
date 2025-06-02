using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.mvc.Models.Groups
{
    public class GroupVM
    {
        public IEnumerable<GetAllDto> Groups { get; set; }
        public string SearchTerm { get; set; }
    }
}
