namespace Grove.Tests.Unit
{
  using System.Linq;
  using Grove.Core.Ai;
  using Infrastructure;
  using Xunit;

  public class BlockStrategyFacts : Scenario
  {
    [Fact]
    public void BlockToMaximizeScore()
    {
      var attackers = Permanents(P1,
        "Shivan Dragon", "Llanowar Behemoth", "Grizzly Bears");

      var blockerCandidates = Permanents(P2,
        "Vampire Nighthawk", "Vampire Nighthawk"
        );

      P2.Life = 20;

      var chosenBlockers = new BlockStrategy(
        attackers,
        blockerCandidates,
        P2.Life
        ).Result.ToList();


      Equal(2, chosenBlockers.Count);
      Equal("Shivan Dragon", chosenBlockers[0].Attacker.Name);
      Equal("Grizzly Bears", chosenBlockers[1].Attacker.Name);
    }


    [Fact]
    public void GangBlock()
    {
      var attackers = Permanents(P1,
        "Elvish Warrior");

      var blockerCandidates = Permanents(P2,
        "Elvish Warrior",
        "Elvish Warrior");

      P2.Life = 20;

      var chosenBlockers = new BlockStrategy(
        attackers,
        blockerCandidates,
        P2.Life).Result;

      Equal(2, chosenBlockers.Count());
    }

    [Fact]
    public void IllegalBlock1()
    {
      var attackers = Permanents(P1, "Shivan Dragon");
      var blockerCandidates = Permanents(P2, "Grizzly Bears");

      P2.Life = 5;

      var chosenBlockers = new BlockStrategy(
        attackers,
        blockerCandidates,
        P2.Life
        ).Result.ToList();

      Equal(0, chosenBlockers.Count);
    }

    [Fact]
    public void TrampleBlock()
    {
      var attackers = Permanents(P1, "Vigor");
      var blockerCandidates = Permanents(P2, "Grizzly Bears");

      P2.Life = 12;

      var chosenBlockers = new BlockStrategy(
        attackers,
        blockerCandidates,
        P2.Life
        ).Result.ToList();

      Equal(0, chosenBlockers.Count);
    }
  }
}