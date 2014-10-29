namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AeronautTinkerer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GetsFlying()
      {
        var tinker = C("Aeronaut Tinkerer");
        Hand(P1, "Profane Memento");
        Battlefield(P1, tinker,"Forest");

        RunGame(1);

        True(C(tinker).Has().Flying);
      }

      [Fact]
      public void HasNotFlying()
      {
        var tinker = C("Aeronaut Tinkerer");
        Battlefield(P1, tinker);

        RunGame(1);

        False(C(tinker).Has().Flying);
      }
    }
  }
}