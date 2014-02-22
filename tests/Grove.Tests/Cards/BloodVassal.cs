namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class BloodVassal
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayEngine()
      {
        var engine = C("Wurmcoil Engine");

        Hand(P1, engine);
        Battlefield(P2, "Llanowar Behemoth");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Blood Vassal");

        RunGame(2);
        Equal(Zone.Battlefield, C(engine).Zone);
      }

      [Fact]
      public void BugManaNotAvailableForRegeneration()
      {
        Hand(P1, "Doom Blade");
        Battlefield(P1, "Unworthy dead", "Blood Vassal");
        Battlefield(P2, "Llanowar Elves", "Llanowar Elves" );

        RunGame(2);

        Equal(17, P2.Life);
      }
    }
  }
}