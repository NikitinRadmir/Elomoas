using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;

namespace Elomoas.Application.Features.Courses.Commands;

public record CreateCourseCommand : IRequest<Course>
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? Img { get; init; }
    public decimal Price { get; init; }
    public ProgramLanguage PL { get; init; }
    public string? Video { get; init; }
    public string? Learn { get; init; }
} 