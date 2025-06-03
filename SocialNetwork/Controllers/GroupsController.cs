using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.Application.Features.Groups.Query.GetAll;
using Elomoas.mvc.Models.Groups;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Application.Features.Groups.Commands.SubscribeToGroup;
using Elomoas.Application.Features.Groups.Commands.UnsubscribeFromGroup;

namespace Elomoas.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IMediator _mediator;

        public GroupsController(ILogger<GroupsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Groups(string search)
        {
            var query = new GetAllQuery();
            var allGroups = await _mediator.Send(query);
            
            // Разделяем группы на подписанные и остальные
            var subscribedGroups = allGroups.Where(g => g.IsCurrentUserSubscribed);
            var otherGroups = allGroups.Where(g => !g.IsCurrentUserSubscribed);
            
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                subscribedGroups = subscribedGroups.Where(g => g.Name.ToLower().Contains(searchLower));
                otherGroups = otherGroups.Where(g => g.Name.ToLower().Contains(searchLower));
            }
            
            var viewModel = new GroupVM
            {
                Groups = otherGroups,
                SubscribedGroups = subscribedGroups,
                SearchTerm = search
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Subscribe(int groupId)
        {
            _logger.LogInformation($"Attempting to subscribe to group {groupId}");
            var command = new SubscribeToGroupCommand(groupId);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"Subscribe result: {result}");
            
            if (!result)
            {
                _logger.LogWarning($"Failed to subscribe to group {groupId}");
                return Json(new { success = false });
            }

            await _mediator.Send(new GetAllQuery()); // Обновляем кэш
            return Json(new { success = true });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int groupId)
        {
            _logger.LogInformation($"Attempting to unsubscribe from group {groupId}");
            var command = new UnsubscribeFromGroupCommand(groupId);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"Unsubscribe result: {result}");

            if (!result)
            {
                _logger.LogWarning($"Failed to unsubscribe from group {groupId}");
                return Json(new { success = false });
            }

            await _mediator.Send(new GetAllQuery()); // Обновляем кэш
            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
