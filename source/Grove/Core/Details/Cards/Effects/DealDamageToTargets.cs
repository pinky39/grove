namespace Grove.Core.Details.Cards.Effects
{
  using System.Linq;
  using Modifiers;
  using Targeting;

  public class DealDamageToTargets : Effect
  {
    public Value  Amount = 0;    
    public bool   GainLife;    

    public override int CalculatePlayerDamage(Player player)
    {
      return Targets.Any(x=> x == player) ? Amount.GetValue(X) : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return Targets.Any(x => x == creature) ? Amount.GetValue(X) : 0;
    }  

    protected override void ResolveEffect()
    {
      foreach (var t in ValidTargets) {
        var damage = new Damage(
          source: Source.OwningCard,
          amount: Amount.GetValue(X),
          isCombat: false,
          changeTracker: Game.ChangeTracker);
                
        t.DealDamage(damage);

        if (GainLife)
          Controller.Life += damage.Amount;
      }
    }

    public override bool NeedsTargets
    {
      get { return true; }
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}