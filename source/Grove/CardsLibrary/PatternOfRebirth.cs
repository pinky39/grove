namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class PatternOfRebirth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pattern of Rebirth")
        .ManaCost("{3}{G}")
        .Type("Enchantment Aura")
        .Text(
          "When enchanted creature dies, that creature's controller may search his or her library for a creature card and put that card onto the battlefield. If that player does, he or she shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectOrCostRankBy(c => c.Toughness.GetValueOrDefault(), ControlledBy.SpellOwner));
            p.TimingRule(new OnFirstMain());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When enchanted creature dies, that creature's controller may search his or her library for a creature card and put that card onto the battlefield. If that player does, he or she shuffles his or her library.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              selector: (c, ctx) => ctx.OwningCard.AttachedTo == c));

            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().Creature,
              text: "Search your library for a creature.",
              player: P(
                e => e.TriggerMessage<ZoneChangedEvent>().Controller,
                EvaluateAt.OnResolve));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}