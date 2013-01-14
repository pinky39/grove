namespace Grove.Core.Effects
{
  public class DealExistingDamageToController : Effect
  {
    public Damage Damage { get; set; }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == Controller ? Damage.Amount : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      Controller.DealDamage(Damage);
    }
  }
}