namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WarNameAspirant
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AspirantEntersWith11()
      {
        var aspirant = C("War-Name Aspirant");
        Battlefield(P1, "Grizzly Bears", "Mountain", "Forest");
        Hand(P1, aspirant);

        RunGame(1);

        Equal(3, C(aspirant).Power);
        Equal(2, C(aspirant).Toughness);
      }

      [Fact]
      public void AspirantCannotBeBlocked()
      {
        Battlefield(P1, "War-Name Aspirant");

        P2.Life = 2;
        Battlefield(P2, "Fugitive Wizard");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
