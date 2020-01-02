namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DisownedAncestor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Pump()
      {
        var ancestor = C("Disowned Ancestor");
        Battlefield(P1, "Swamp", "Swamp", ancestor);

        RunGame(1);
        
        Equal(1, ancestor.Card.Power);
        Equal(5, ancestor.Card.Toughness);
      }
    }
  }
}