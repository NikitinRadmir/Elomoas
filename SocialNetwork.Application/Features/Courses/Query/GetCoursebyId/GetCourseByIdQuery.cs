using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Elomoas.Application.Features.Courses.Query;

namespace Elomoas.Application.Features.Courses.Query.GetCourseById;

public record GetCourseByIdQuery(int id) : IRequest<CourseDto>;
