namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ElvishHerder
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveTrampleToForce()
      {
        var force = C("Verdant Force");
        Battlefield(P1, force, "Elvish Herder", "Forest");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 5;

        RunGame(1);

        True(C(force).Has().Trample);
      }
    }
  }
}