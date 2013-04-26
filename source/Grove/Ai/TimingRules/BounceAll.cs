namespace Grove.Ai.TimingRules
{
  using System.Linq;
  using Gameplay.States;

  public class BounceAll : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step == Step.CombatDamage && p.Controller.IsActive)
      {
        var controllerCount = p.Controller.Battlefield.Creatures.Count(x => !x.IsTapped);
        var opponentCount = p.Controller.Opponent.Battlefield.Creatures.Count();

        return controllerCount < opponentCount;
      }

      return Turn.Step == Step.DeclareBlockers && !p.Controller.IsActive &&
        Combat.Attackers.Any();
    }
  }
}