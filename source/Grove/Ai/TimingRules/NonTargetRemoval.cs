namespace Grove.Ai.TimingRules
{
  using System.Linq;
  using Core;
  using Gameplay.States;

  public class NonTargetRemoval : TimingRule
  {
    private readonly int _count;

    private NonTargetRemoval() {}

    public NonTargetRemoval(int count)
    {
      _count = count;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Card.IsPermanent || p.Card.Is().Instant
        ? Instant(p)
        : Sorcery(p);
    }

    private bool Instant(TimingRuleParameters p)
    {
      var opponentCreatureCount = p.Controller.Opponent.Battlefield.Creatures.Count();

      if (opponentCreatureCount == 0)
        return false;

      if (opponentCreatureCount == 1)
      {
        return (!p.Controller.IsActive && Turn.Step == Step.DeclareBlockers) ||
          (p.Controller.IsActive && Turn.Step == Step.DeclareAttackers);
      }

      if (opponentCreatureCount > 2*_count + 1)
        return false;

      return !p.Controller.IsActive && Turn.Step == Step.EndOfTurn;
    }


    private bool Sorcery(TimingRuleParameters p)
    {
      var opponentCreatureCount = p.Controller.Opponent.Battlefield.Creatures.Count();

      if (opponentCreatureCount == 0)
        return false;

      if (opponentCreatureCount == 1)
        return Turn.Step == Step.FirstMain;

      if (opponentCreatureCount > 2*_count + 1)
        return false;

      return Turn.Step == Step.SecondMain;
    }
  }
}