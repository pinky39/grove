namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StaffOfTheSunMagus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GetLives()
      {
        // Casting white spell and playing land don't increase life of P2
        Hand(P1, "Plains", "Oreskos Swiftclaw");
        Battlefield(P1, "Plains");

        // Casting white spell and playing land increase life of P2
        Library(P2, "Plains");
        Hand(P2, "Oreskos Swiftclaw");
        Battlefield(P2, "Staff of the Sun Magus", "Plains", "Plains");

        RunGame(2);       

        Equal(0, P1.Hand.Count);
        Equal(0, P2.Hand.Count);
        Equal(22, P2.Life);
      }
    }
  }
}
