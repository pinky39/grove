namespace Grove.Core.Details.Cards.Effects
{
  using Ai;

  public class DealExistingDamageToController : Effect, IDamageDealing
  {
    public Damage Damage { get; set; }

    public int PlayerDamage(Player player)
    {
      return player == Controller ? Damage.Amount : 0;
    }

    public int CreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      Controller.DealDamage(Damage);
    }
  }
}