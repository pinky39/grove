namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MultaniMaroSorcerer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Is88()
      {
        var multani = C("Multani, Maro-Sorcerer");
        
        Battlefield(P1, multani, "Swamp", "Swamp");
        
        Hand(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Sign In Blood");
        Hand(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(8, C(multani).Power);
        Equal(8, C(multani).Toughness);
      }
    }
  }
}