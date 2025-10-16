using CardActionsService.Enums;
using CardActionsService.Models;

namespace CardActionsService.Services;

public class AllowedCardActionsService
{
    private readonly ILogger<AllowedCardActionsService> _logger;

    public AllowedCardActionsService(ILogger<AllowedCardActionsService> logger)
    {
        _logger = logger;
    }

    private sealed record CardActionsRules(string ActionName, Func<CardDetails, bool> Condition);

    // Easy adding new Actions
    private static readonly List<CardActionsRules> ActionRules = new()
    {
        new("ACTION3", _ => true),
        new("ACTION4", _ => true),
        new("ACTION9", _ => true),
        new("ACTION1", card => card.CardStatus == CardStatus.Active),
        new("ACTION2", card => card.CardStatus == CardStatus.Inactive),
        new("ACTION5", card => card.CardType == CardType.Credit),
        new("ACTION6", card =>
            (card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active or CardStatus.Blocked)
            && card.IsPinSet),
        new("ACTION7", card =>
            ((card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active) && !card.IsPinSet) ||
            card is { CardStatus: CardStatus.Blocked, IsPinSet: true }),
        new("ACTION8",
            card => card.CardStatus is not (CardStatus.Restricted or CardStatus.Expired or CardStatus.Closed)),
        new("ACTION10", card => card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active),
        new("ACTION11", card => card.CardStatus is CardStatus.Inactive or CardStatus.Active),
        new("ACTION12", card => card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active),
        new("ACTION13", card => card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active)
    };

    public List<string> GetAllowedCardActions(CardDetails card)
    {
        if (card == null)
        {
            _logger.LogWarning("Attempted to get allowed actions for a null card.");
            throw new ArgumentNullException(nameof(card), "Card details must be provided.");
        }

        var allowedActions = new List<string>();

        foreach (var rule in ActionRules)
        {
            try
            {
                if (rule.Condition(card))
                    allowedActions.Add(rule.ActionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating rule {Rule} for card {CardNumber}", rule.ActionName,
                    card.CardNumber);
                throw new InvalidOperationException($"Rule '{rule.ActionName}' failed.", ex);
            }
        }

        return allowedActions.Distinct().ToList();
    }
}