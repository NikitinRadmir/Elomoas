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
        public string? Name { get; set; } = "New Course";
        public string? Description { get; set; }
        public string? Img { get; set; } = "/images/v-1.png";
        public decimal Price { get; set; }
        public ProgramLanguage PL { get; set; }
        public string? Video { get; set; } = "/images/video4.mp4";
        public string? Learn { get; set; }
        public List<ProgramLanguage> AvailableLanguages { get; set; }
    }

    public class UpdateCourseViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? Img { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Programming Language is required")]
        public ProgramLanguage PL { get; set; }
        
        public string? Video { get; set; }
        
        public string? Learn { get; set; }
        
        public List<ProgramLanguage> AvailableLanguages { get; set; }
    }
} 