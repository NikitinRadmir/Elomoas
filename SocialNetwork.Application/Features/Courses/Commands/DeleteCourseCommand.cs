using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Courses.Commands;

public record DeleteCourseCommand(int Id) : IRequest<bool>;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly ICourseService _courseService;

    public DeleteCourseCommandHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        await _courseService.DeleteCourseAsync(request.Id);
        return true;
    }
} 