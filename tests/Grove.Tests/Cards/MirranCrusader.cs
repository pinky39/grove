namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class MirranCrusader
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoubleStrike1()
      {
        Battlefield(P1, "Mirran Crusader", "Mirran Crusader", "Llanowar Elves");
        Battlefield(P2, "Savannah Lions", "Savannah Lions");

        P2.Life = 4;

        RunGame(maxTurnCount: 1);

        Equal(0, P2.Battlefield.Count());
        Equal(3, P1.Battlefield.Count());
        Equal(3, P2.Life);        
        
      }

      [Fact]
      public void DoubleStrike2()
      {
        Battlefield(P1, "Mirran Crusader", "Llanowar Elves");
        Battlefield(P2, "Savannah Lions", "Savannah Lions");

        P2.Life = 20;

        RunGame(maxTurnCount: 1);

        Equal(0, P2.Battlefield.Count());
        Equal(1, P1.Battlefield.Count());
        Equal(20, P2.Life);        
      }
      
    }

    public class Predefined : PredifinedScenario
    {
      [Fact]
      public void RemoveFromCombatAfterLeathalFirstStrikeDamage()
      {
        var troll = C("Troll Ascetic");
        var mirran = C("Mirran Crusader");

        Battlefield(P1, troll);
        Battlefield(P2, mirran);        
        
        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(troll),
          At(Step.DeclareBlockers)
            .DeclareBlockers(troll, mirran)
            .Activate(troll),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Battlefield, C(troll).Zone);
                True(C(troll).IsTapped);
              })
        );

      }      
    }
  }
}