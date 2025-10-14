using CardActionsService.Enums;
using CardActionsService.Models;
using CardActionsService.Services;

namespace CardActionsService.Tests.Services;

public class AllowedActionsServiceTests
{
    private readonly AllowedCardActionsService _service = new();

    [Theory]
    // Prepaid
    [InlineData(CardType.Prepaid, CardStatus.Ordered, false, new[] { "ACTION3","ACTION4","ACTION7","ACTION8","ACTION9","ACTION10","ACTION12","ACTION13" })]
    [InlineData(CardType.Prepaid, CardStatus.Ordered, true, new[] { "ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION12","ACTION13" })]
    [InlineData(CardType.Prepaid, CardStatus.Inactive, false, new[] { "ACTION2","ACTION3","ACTION4","ACTION7","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Prepaid, CardStatus.Inactive, true, new[] { "ACTION2","ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Prepaid, CardStatus.Active, false, new[] { "ACTION1","ACTION3","ACTION4","ACTION7","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Prepaid, CardStatus.Active, true, new[] { "ACTION1","ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Prepaid, CardStatus.Restricted, false, new[] { "ACTION3","ACTION4","ACTION9" })]
    [InlineData(CardType.Prepaid, CardStatus.Blocked, false, new[] { "ACTION3","ACTION4","ACTION8","ACTION9" })]
    [InlineData(CardType.Prepaid, CardStatus.Blocked, true, new[] { "ACTION3","ACTION4","ACTION6","ACTION7","ACTION8","ACTION9" })]
    [InlineData(CardType.Prepaid, CardStatus.Expired, false, new[] { "ACTION3","ACTION4","ACTION9" })]
    [InlineData(CardType.Prepaid, CardStatus.Closed, false, new[] { "ACTION3","ACTION4","ACTION9" })]

    // Debit
    [InlineData(CardType.Debit, CardStatus.Ordered, false, new[] { "ACTION3","ACTION4","ACTION7","ACTION8","ACTION9","ACTION10","ACTION12","ACTION13" })]
    [InlineData(CardType.Debit, CardStatus.Ordered, true, new[] { "ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION12","ACTION13" })]
    [InlineData(CardType.Debit, CardStatus.Inactive, false, new[] { "ACTION2","ACTION3","ACTION4","ACTION7","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Debit, CardStatus.Inactive, true, new[] { "ACTION2","ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Debit, CardStatus.Active, false, new[] { "ACTION1","ACTION3","ACTION4","ACTION7","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Debit, CardStatus.Active, true, new[] { "ACTION1","ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Debit, CardStatus.Restricted, false, new[] { "ACTION3","ACTION4","ACTION9" })]
    [InlineData(CardType.Debit, CardStatus.Blocked, false, new[] { "ACTION3","ACTION4","ACTION8","ACTION9" })]
    [InlineData(CardType.Debit, CardStatus.Blocked, true, new[] { "ACTION3","ACTION4","ACTION6","ACTION7","ACTION8","ACTION9" })]
    [InlineData(CardType.Debit, CardStatus.Expired, false, new[] { "ACTION3","ACTION4","ACTION9" })]
    [InlineData(CardType.Debit, CardStatus.Closed, false, new[] { "ACTION3","ACTION4","ACTION9" })]

    // Credit
    [InlineData(CardType.Credit, CardStatus.Ordered, false, new[] { "ACTION3","ACTION4","ACTION5","ACTION7","ACTION8","ACTION9","ACTION10","ACTION12","ACTION13" })]
    [InlineData(CardType.Credit, CardStatus.Ordered, true, new[] { "ACTION3","ACTION4","ACTION5","ACTION6","ACTION8","ACTION9","ACTION10","ACTION12","ACTION13" })]
    [InlineData(CardType.Credit, CardStatus.Inactive, false, new[] { "ACTION2","ACTION3","ACTION4","ACTION5","ACTION7","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Credit, CardStatus.Inactive, true, new[] { "ACTION2","ACTION3","ACTION4","ACTION5","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Credit, CardStatus.Active, false, new[] { "ACTION1","ACTION3","ACTION4","ACTION5","ACTION7","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Credit, CardStatus.Active, true, new[] { "ACTION1","ACTION3","ACTION4","ACTION5","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" })]
    [InlineData(CardType.Credit, CardStatus.Restricted, false, new[] { "ACTION3","ACTION4","ACTION5","ACTION9" })]
    [InlineData(CardType.Credit, CardStatus.Blocked, false, new[] { "ACTION3","ACTION4","ACTION5","ACTION8","ACTION9" })]
    [InlineData(CardType.Credit, CardStatus.Blocked, true, new[] { "ACTION3","ACTION4","ACTION5","ACTION6","ACTION7","ACTION8","ACTION9" })]
    [InlineData(CardType.Credit, CardStatus.Expired, false, new[] { "ACTION3","ACTION4","ACTION5","ACTION9" })]
    [InlineData(CardType.Credit, CardStatus.Closed, false, new[] { "ACTION3","ACTION4","ACTION5","ACTION9" })]
    public void GetAllowedActions_ReturnsExpectedActions(CardType type, CardStatus status, bool isPinSet, string[] expected)
    {
        // Arrange
        var card = new CardDetails("TestCard", type, status, isPinSet);

        // Act
        var actions = _service.GetAllowedCardActions(card);

        // Assert
        Assert.Equal(expected.OrderBy(a => a), actions.OrderBy(a => a));
    }
}
