namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class MentalDiscipline
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Discard2Islands()
      {
        Hand(P1, "Island", "Island", "Island");
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island", "Mental Discipline");
        
        RunGame(2);

        Equal(2, P1.Graveyard.Count(c => c.Is("island")));
      }
    }
  }
}