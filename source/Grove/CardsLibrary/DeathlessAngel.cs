namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.Modifiers;

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
            p.Cost = new PayMana("{W}{W}".Parse());

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Indestructible) {UntilEot = true})
              .SetTags(EffectTag.Indestructible);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectGiveIndestructible());
          });
    }
  }
}