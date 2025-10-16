using CardActionsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardActionsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardActionsController : ControllerBase
{
    private readonly CardService _cardService;
    private readonly AllowedCardActionsService _allowedCardActionsService;
    private readonly ILogger<CardActionsController> _logger;

    public CardActionsController(
        CardService cardService,
        AllowedCardActionsService allowedCardActionsService,
        ILogger<CardActionsController> logger)
    {
        _cardService = cardService;
        _allowedCardActionsService = allowedCardActionsService;
        _logger = logger;
    }

    [HttpGet("{userId}/{cardNumber}/allowed")]
    public async Task<IActionResult> GetAllowedActions(string userId, string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(cardNumber))
            throw new ArgumentException("UserId and CardNumber must be provided");

        var card = await _cardService.GetCardDetails(userId, cardNumber);
        if (card == null)
            throw new KeyNotFoundException($"Card '{cardNumber}' not found for user '{userId}'");

        var actions = _allowedCardActionsService.GetAllowedCardActions(card);

        _logger.LogInformation("Returned {ActionCount} actions for user {UserId} card {CardNumber}",
            actions.Count, userId, cardNumber);

        return Ok(new { cardNumber = card.CardNumber, allowedActions = actions });
    }
}