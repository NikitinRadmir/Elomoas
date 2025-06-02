using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.Application.Features.Groups.Query.GetAll;
using Elomoas.mvc.Models.Groups;

namespace Elomoas.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IMediator _mediator;

        public GroupsController(ILogger<GroupsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Groups()
        {
            var query = new GetAllQuery();
            var viewModel = new GroupVM
            {
                Groups = await _mediator.Send(query)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> GroupsConfig()
        {
            var query = new GetAllQuery();
            var viewModel = new GroupVM
            {
                Groups = await _mediator.Send(query)
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(int groupId)
        {
            var result = await _mediator.Send(new SubscribeCommand(groupId));

            if (!result)
            {
                _logger.LogWarning($"Failed to subscribe to group {groupId}");
                return Json(new { success = false });
            }

            await _mediator.Send(new GetAllQuery());
            return Json(new { success = true });
        }
    }
} 