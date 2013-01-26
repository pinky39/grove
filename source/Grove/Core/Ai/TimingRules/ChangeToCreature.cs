namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;
  using Mana;

  public class ChangeToCreature : TimingRule
  {
    public int MinAvailableMana;

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step == Step.BeginningOfCombat && p.Controller.IsActive)
      {
        return MinAvailableMana == 0 || p.Controller.HasMana(MinAvailableMana, ManaUsage.Abilities);
      }

      if (Turn.Step == Step.DeclareAttackers && !p.Controller.IsActive)
      {
        return MinAvailableMana == 0 ||
          p.Controller.HasMana(MinAvailableMana, ManaUsage.Abilities) && Combat.Attackers.Any();
      }

      return false;
    }
  }
}