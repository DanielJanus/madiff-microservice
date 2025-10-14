using CardActionsService.Services;

namespace CardActionsService.Tests.Services;

public class CardServiceTests
{
    private readonly CardService _service = new();

    [Theory]
    [InlineData("User1", "Card12", true)]
    [InlineData("User2", "Card25", true)]
    [InlineData("User3", "Card311", true)]
    [InlineData("User999", "Card12", false)] 
    [InlineData("User1", "NoCard", false)]  
    [InlineData("User4", "Card22", false)]  
    [InlineData("User1", "", false)]  
    [InlineData("", "", false)]  
    [InlineData("", "Card11", false)]  
    public async Task GetCardDetails_TestCases(string userId, string cardNumber, bool exists)
    {
        var card = await _service.GetCardDetails(userId, cardNumber);

        if (exists)
        {
            Assert.NotNull(card);
            Assert.Equal(cardNumber, card!.CardNumber);
        }
        else
        {
            Assert.Null(card);
        }
    }
}