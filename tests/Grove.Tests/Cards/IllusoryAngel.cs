namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class IllusoryAngel
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastAngelAfterElf()
      {        
        Hand(P1, "Illusory Angel", "Llanowar Elves");
        Battlefield(P1, "Forest","Forest","Island", "Island");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count());
      }

      [Fact]
      public void CannotCastAngelIfHaveNoOtherSpell()
      {
        Hand(P1, "Illusory Angel");
        Battlefield(P1, "Forest", "Forest", "Island", "Island");

        RunGame(1);

        Equal(0, P1.Battlefield.Creatures.Count());
      }
    }
  }
}