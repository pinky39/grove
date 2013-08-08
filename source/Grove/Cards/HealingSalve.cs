namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class HealingSalve : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new Gameplay.Effects.PreventNextDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new Artifical.TargetingRules.PreventNextDamageToTargets(3));
          });
    }
  }
}