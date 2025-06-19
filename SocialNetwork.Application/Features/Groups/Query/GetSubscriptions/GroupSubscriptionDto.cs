using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Groups.Query.GetSubscriptions
{
    public class GroupSubscriptionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
    }
}
