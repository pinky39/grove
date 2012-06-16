namespace Grove.Tests
{
  using System;
  using Infrastructure;
  using Xunit;

  public class CardListGenerator : Scenario
  {
    [Fact]
    public void Generate()
    {
      foreach (var cardName in CardDatabase.GetAvailableCardsNames())
      {
        if (cardName != "Uncastable")
          Console.WriteLine(cardName);
      } 
    }
  }
}