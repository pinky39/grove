namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class HauntedPlateMail
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ChangePlateToCreature()
      {
        Battlefield(P1, "Haunted Plate Mail", "Plains", "Plains", "Plains", "Plains");

        RunGame(1);

        Equal(16, P2.Life);        
      }

      [Fact]
      public void CannotChangePlateToCreature()
      {
        Battlefield(P1, "Grizzly Bears", "Haunted Plate Mail");

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(2, P2.Life);
      }

      [Fact]
      public void CannotActivateMailEndless()
      {
        Battlefield(P1, "Haunted Plate Mail");
        Battlefield(P2, "Haunted Plate Mail");

        RunGame(1);
      }
    }
  }
}
