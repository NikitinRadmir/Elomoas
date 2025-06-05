using Elomoas.Application.Features.Courses.Query;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Areas.Admin.Models
{
    public class CoursesViewModel
    {
        public IEnumerable<CourseDto> Courses { get; set; }
    }

    public class CreateCourseViewModel
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Img { get; set; } = "/images/v-1.png";

        public decimal Price { get; set; }

        public ProgramLanguage PL { get; set; }

        public string? Video { get; set; } = "/images/video4.mp4";

        public string? Learn { get; set; }

        public List<ProgramLanguage> AvailableLanguages { get; set; }
    }

    public class UpdateCourseViewModel : CreateCourseViewModel
    {
        public int Id { get; set; }
    }
} 