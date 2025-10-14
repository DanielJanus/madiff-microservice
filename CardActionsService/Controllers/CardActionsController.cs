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

    public CardActionsController(CardService cardService, AllowedCardActionsService allowedCardActionsService, ILogger<CardActionsController> logger)
    {
        _cardService = cardService;
        _allowedCardActionsService = allowedCardActionsService;
        _logger = logger;
    }
    
    [HttpGet("{userId}/{cardNumber}/allowed")]
    public async Task<IActionResult> GetAllowedActions(string userId, string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(cardNumber))
        {
            _logger.LogWarning("Attempt to get the data with empty userId or cardNumber");
            return BadRequest(new { error = "UserId and CardNumber must be provided" });
        }

        var card = await _cardService.GetCardDetails(userId, cardNumber);
        if (card == null)
        {
            _logger.LogWarning("Card {CardNumber} not found for user {UserId}", cardNumber, userId);
            return NotFound(new { error = "Card or User not found" });
        }

        var actions = _allowedCardActionsService.GetAllowedCardActions(card);
        return Ok(new { cardNumber = card.CardNumber, allowedActions = actions });
    }

}