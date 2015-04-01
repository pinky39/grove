namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class EternalThirst
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void JuggernautGetsCounter()
      {
        var juggernaut = C("Juggernaut").IsEnchantedWith("Eternal Thirst");
        Battlefield(P1, juggernaut);

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(6, C(juggernaut).Power);
      }
    }
  }
}