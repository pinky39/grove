namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;
  using Mana;

  public class ChangeToCreature : TimingRule
  {
    private readonly int _minAvailableMana;

    private ChangeToCreature() {}

    public ChangeToCreature(int minAvailableMana = 0)
    {
      _minAvailableMana = minAvailableMana;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step == Step.BeginningOfCombat && p.Controller.IsActive)
      {
        return _minAvailableMana == 0 || p.Controller.HasMana(_minAvailableMana, ManaUsage.Abilities);
      }

      if (Turn.Step == Step.DeclareAttackers && !p.Controller.IsActive)
      {
        return _minAvailableMana == 0 ||
          p.Controller.HasMana(_minAvailableMana, ManaUsage.Abilities) && Combat.Attackers.Any();
      }

      return false;
    }
  }
}