namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StaffOfTheSunMagus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain2Life()
      {        
        Hand(P1, "Plains", "Oreskos Swiftclaw");                
        Battlefield(P1, "Staff of the Sun Magus", "Plains", "Plains");

        RunGame(2);               
        Equal(22, P1.Life);
      }
    }
  }
}
