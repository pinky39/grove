namespace Grove.Effects
{
  using System.Linq;
  using Modifiers;

  public class EachTargetDealsDamageEqualToItsToughnessToOther : Effect
  {
    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };

      var attacker = ValidEffectTargets.ToList()[0].Card();
      var blocker = ValidEffectTargets.ToList()[1].Card();

      attacker.DealDamageTo(attacker.Toughness.GetValueOrDefault(), blocker, false);
      blocker.DealDamageTo(blocker.Toughness.GetValueOrDefault(), attacker, false);
    }
  }
}
