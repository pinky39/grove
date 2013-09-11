namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class DeathlessAngel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Deathless Angel")
        .ManaCost("{4}{W}{W}")
        .Type("Creature Angel")
        .Text("{Flying}{EOL}{W}{W}: Target creature is indestructible this turn.")
        .FlavorText(
          "I should have died that day, but I suffered not a scratch. I awoke in a lake of blood, none of it apparently my own.")
        .Power(5)
        .Toughness(7)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{W}{W}: Target creature is indestructible this turn.";
            p.Cost = new PayMana("{W}{W}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Indestructible)).Tags(EffectTag.Indestructible);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectGiveIndestructible());
          });
    }
  }
}