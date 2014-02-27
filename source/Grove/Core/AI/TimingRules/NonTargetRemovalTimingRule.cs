namespace Grove.AI.TimingRules
{
  using System.Linq;

  public class NonTargetRemovalTimingRule : TimingRule
  {
    private readonly int _count;

    private NonTargetRemovalTimingRule() {}

    public NonTargetRemovalTimingRule(int count)
    {
      _count = count;
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
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
        return (IsBeforeYouDeclareBlockers(p.Controller) || IsBeforeYouDeclareAttackers(p.Controller));
      }

      if (opponentCreatureCount > 2*_count + 1)
        return false;

      return IsEndOfOpponentsTurn(p.Controller);
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