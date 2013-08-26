namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.DamageHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Worship : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Worship")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "If you control a creature, damage that would reduce your life total to less than 1 reduces it to 1 instead.")
        .FlavorText("Believe in the ideal, not the idol.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .StaticAbility(p => p.Modifier(
          () => new AddDamagePrevention(modifier => new PreventLifelossBelowOneToPlayer(modifier.SourceCard.Controller))));
    }
  }
}