namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class DyingWail : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dying Wail")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text("When enchanted creature dies, target player discards two cards.")
        .FlavorText("This is a world of spiteful wasps that sting and kill even as they die.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectOrCostRankBy(c => c.Score, ControlledBy.SpellOwner));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When enchanted creature dies, target player discards two cards.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              selector: (c, ctx) => ctx.OwningCard.AttachedTo == c));

            p.Effect = () => new DiscardCards(2);
            p.TargetSelector.AddEffect(s => s.Is.Player());
            p.TargetingRule(new EffectOpponent());

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}