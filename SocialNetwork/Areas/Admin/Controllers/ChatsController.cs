using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.GetAllChats;
using Elomoas.Application.Features.Messenger.Queries.GetChatById;
using Elomoas.Application.Features.Messenger.Commands.CreateChat;
using Elomoas.Application.Features.Messenger.Commands.UpdateChat;
using Elomoas.Application.Features.Messenger.Commands.DeleteChat;
using SocialNetwork.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Areas.Admin.Controllers;

[Area("Admin")]
public class ChatsController : Controller
{
    private readonly IMediator _mediator;
    private readonly UserManager<IdentityUser> _userManager;

    public ChatsController(IMediator mediator, UserManager<IdentityUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var chats = await _mediator.Send(new GetAllChatsQuery());
        var viewModel = new ChatsViewModel { Chats = chats };
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var chat = await _mediator.Send(new GetChatByIdQuery(id));
        if (chat == null)
        {
            return NotFound();
        }
        return View(chat);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _userManager.Users.ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateChatViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateChatCommand
            {
                User1Id = viewModel.User1Id,
                User2Id = viewModel.User2Id
            };

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Users = await _userManager.Users.ToListAsync();
        return View(viewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var chat = await _mediator.Send(new GetChatByIdQuery(id));
        if (chat == null)
        {
            return NotFound();
        }

        var viewModel = new UpdateChatViewModel
        {
            Id = chat.Id,
            User1Id = chat.User1Id,
            User2Id = chat.User2Id
        };

        ViewBag.Users = await _userManager.Users.ToListAsync();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateChatViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var command = new UpdateChatCommand
            {
                Id = viewModel.Id,
                User1Id = viewModel.User1Id,
                User2Id = viewModel.User2Id
            };

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Users = await _userManager.Users.ToListAsync();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteChatCommand(id));
        return RedirectToAction(nameof(Index));
    }
} 