using CardActionsService.Enums;
using CardActionsService.Models;

namespace CardActionsService.Services;

public class AllowedCardActionsService
{
    private sealed record CardActionsRules(string ActionName, Func<CardDetails, bool> Condition);

    // Easy adding new Actions
    private static readonly List<CardActionsRules> ActionRules = new()
    {
        new CardActionsRules("ACTION3", _ => true),
        new CardActionsRules("ACTION4", _ => true),
        new CardActionsRules("ACTION9", _ => true),
        new CardActionsRules("ACTION1", card => card.CardStatus == CardStatus.Active),
        new CardActionsRules("ACTION2", card => card.CardStatus == CardStatus.Inactive),
        new CardActionsRules("ACTION5", card => card.CardType == CardType.Credit),
        new CardActionsRules("ACTION6", card =>
            (card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active or CardStatus.Blocked)
            && card.IsPinSet),
        new CardActionsRules("ACTION7", card =>
            ((card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active) && !card.IsPinSet) ||
            card is { CardStatus: CardStatus.Blocked, IsPinSet: true }),
        new CardActionsRules("ACTION8", card => card.CardStatus is not (CardStatus.Restricted or CardStatus.Expired or CardStatus.Closed)),
        new CardActionsRules("ACTION10", card => card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active),
        new CardActionsRules("ACTION11", card => card.CardStatus is CardStatus.Inactive or CardStatus.Active),
        new CardActionsRules("ACTION12", card => card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active),
        new CardActionsRules("ACTION13", card => card.CardStatus is CardStatus.Ordered or CardStatus.Inactive or CardStatus.Active)
    };

    public List<string> GetAllowedCardActions(CardDetails card)
    {
        return ActionRules
            .Where(r => r.Condition(card))
            .Select(r => r.ActionName)
            .Distinct()
            .ToList();
    }
}