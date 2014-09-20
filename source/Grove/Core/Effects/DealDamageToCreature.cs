namespace Grove.Effects
{
  public class DealDamageToCreature : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly DynParam<Card> _creature;

    private DealDamageToCreature()
    {
    }

    public DealDamageToCreature(DynParam<int> amount, DynParam<Card> creature)
    {
      _amount = amount;
      _creature = creature;

      RegisterDynamicParameters(creature, amount);
    }

    public override int CalculateCreatureDamage(Card card)
    {
      return card == _creature.Value ? _amount.Value : 0;
    }
    
    protected override void ResolveEffect()
    {
      Source.OwningCard.DealDamageTo(
        _amount.Value,
        _creature.Value,
        isCombat: false);
    }
  }
}