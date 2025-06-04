using Elomoas.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elomoas.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
    }
}
