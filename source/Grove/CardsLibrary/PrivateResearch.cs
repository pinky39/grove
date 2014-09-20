namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class PrivateResearch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Private Research")
        .ManaCost("{U}")
        .Type("Enchantment Aura")
        .Text(
          "At the beginning of your upkeep, you may put a page counter on Private Research.{EOL}When enchanted creature dies, draw a card for each page counter on Private Research.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectRankBy(c => c.Toughness.GetValueOrDefault(), ControlledBy.SpellOwner));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a page counter on Private Research.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Page), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When enchanted creature dies, draw a card for each page counter on Private Research.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard,
              filter: (c, a, _) => c == a.OwningCard.AttachedTo));

            p.Effect = () => new DrawCards(P(e => e.Source.OwningCard.CountersCount(CounterType.Page)));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}