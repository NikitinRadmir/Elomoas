using MediatR;

namespace Elomoas.Application.Features.Courses.Commands;

public record DeleteCourseCommand(int Id) : IRequest<bool>; 