namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class VolcanicHammer : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Volcanic Hammer")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Volcanic Hammer deals 3 damage to target creature or player.")
        .FlavorText("Fire finds its form in the heat of the forge.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new DealDamage(3));
          });
    }
  }
}