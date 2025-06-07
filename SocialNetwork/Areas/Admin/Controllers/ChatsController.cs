using Microsoft.AspNetCore.Mvc;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.GetAllChats;
using Elomoas.Application.Features.Messenger.Queries.GetChatById;
using Elomoas.Application.Features.Messenger.Commands.CreateChat;
using Elomoas.Application.Features.Messenger.Commands.UpdateChat;
using Elomoas.Application.Features.Messenger.Commands.DeleteChat;
using Elomoas.Application.Features.Messenger.Commands.AddMessage;
using Elomoas.Application.Features.Messenger.Commands.UpdateMessage;
using Elomoas.Application.Features.Messenger.Commands.DeleteMessage;
using Elomoas.Application.Features.AppUsers.Queries.GetAllUsers;
using SocialNetwork.Areas.Admin.Models;

namespace SocialNetwork.Areas.Admin.Controllers;

[Area("Admin")]
public class ChatsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatsController> _logger;

    public ChatsController(IMediator mediator, ILogger<ChatsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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
        ViewBag.Users = await _mediator.Send(new GetAllUsersQuery());
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

        ViewBag.Users = await _mediator.Send(new GetAllUsersQuery());
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

        ViewBag.Users = await _mediator.Send(new GetAllUsersQuery());
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

        ViewBag.Users = await _mediator.Send(new GetAllUsersQuery());
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteChatCommand(id));
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMessage(int chatId, string senderId, string content)
    {
        var command = new AddMessageCommand
        {
            ChatId = chatId,
            SenderId = senderId,
            Content = content
        };

        await _mediator.Send(command);
        return RedirectToAction(nameof(Details), new { id = chatId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateMessage(int messageId, int chatId, string content, bool isRead)
    {
        var command = new UpdateMessageCommand
        {
            Id = messageId,
            Content = content,
            IsRead = isRead
        };

        await _mediator.Send(command);
        return RedirectToAction(nameof(Details), new { id = chatId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMessage(int id, int chatId)
    {
        await _mediator.Send(new DeleteMessageCommand(id));
        return RedirectToAction(nameof(Details), new { id = chatId });
    }
} 