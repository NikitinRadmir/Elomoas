using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;
using MediatR;

namespace Elomoas.Application.Features.Courses.Commands
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, bool>
    {
        private readonly ICourseRepository _courseRepository;

        public CreateCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<bool> Handle(CreateCourseCommand command, CancellationToken ct)
        {
            return await _courseRepository.AddCourseAsync(
                command.Name,
                command.Description,
                command.Img,
                command.Price,
                command.PL,
                command.Video,
                command.Learn
            );
        }
    }
} 