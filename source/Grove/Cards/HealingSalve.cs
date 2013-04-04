namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Preventions;

  public class HealingSalve : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Healing Salve")
        .ManaCost("{W}")
        .Type("Instant")
        .Text(
          "Choose one — Target player gains 3 life; or prevent the next 3 damage that would be dealt to target creature or player this turn.")
        .FlavorText(
          "Xantcha is recovering. The medicine is slow, but my magic would have killed her.")
        .Cast(p =>
          {
            p.Text = "Target player gains 3 life";
            p.Effect = () => new TargetPlayerGainsLife(3);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new SpellOwner());
          })
        .Cast(p =>
          {
            p.Text = "Prevent the next 3 damage that would be dealt to target creature or player this turn.";
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddDamagePrevention(new PreventDamage(3)) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new PreventDamageToTargets(3));
          });
    }
  }
}