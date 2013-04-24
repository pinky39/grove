namespace Grove.Core.Effects
{
  public class DealExistingDamageToController : Effect
  {
    private readonly DynParam<Damage> _damage;

    private DealExistingDamageToController() {}

    public DealExistingDamageToController(DynParam<Damage> damage)
    {
      _damage = damage;

      RegisterDynamicParameters(damage);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == Controller ? _damage.Value.Amount : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      Controller.DealDamage(_damage.Value);
    }
  }
}