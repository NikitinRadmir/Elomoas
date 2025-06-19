using Elomoas.mvc.Models.Groups;

namespace Elomoas.mvc.Models.Groups
{
    public class GroupVM
    {
        public IEnumerable<GroupCardVM> Groups { get; set; }
        public IEnumerable<GroupCardVM> SubscribedGroups { get; set; }
        public string SearchTerm { get; set; }
    }
}
