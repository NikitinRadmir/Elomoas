using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Elomoas.Domain.Entities.Enum;

namespace Elomoas.Application.Features.Courses.Commands
{
    public class CreateCourseCommand : IRequest<bool>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Img { get; set; }
        public decimal Price { get; set; }
        public ProgramLanguage PL { get; set; }
        public string? Video { get; set; }
        public string? Learn { get; set; }
    }
} 