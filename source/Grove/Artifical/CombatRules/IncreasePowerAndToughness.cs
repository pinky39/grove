namespace Grove.Artifical.CombatRules
{
  using Gameplay.ManaHandling;

  public class IncreasePowerAndToughness : CombatRule
  {
    private readonly IManaAmount _cost;
    private readonly int _power;
    private readonly int _toughness;

    private IncreasePowerAndToughness() {}

    public IncreasePowerAndToughness(int power, int toughness, IManaAmount cost)
    {
      _power = power;
      _toughness = toughness;
      _cost = cost;
    }


    public override void Apply(CombatAbilities combatAbilities)
    {
      var cost = _cost;

      while (OwningCard.Controller.HasMana(cost, ManaUsage.Abilities))
      {
        combatAbilities.PowerChange += _power;
        combatAbilities.ToughnessChange += _toughness;

        cost = cost.Add(_cost);
      }
    }
  }
}