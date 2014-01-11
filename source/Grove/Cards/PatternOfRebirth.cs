namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

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
            p.TargetingRule(new EffectRankBy(c => c.Toughness.GetValueOrDefault(), ControlledBy.SpellOwner));
            p.TimingRule(new OnFirstMain());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When enchanted creature dies, that creature's controller may search his or her library for a creature card and put that card onto the battlefield. If that player does, he or she shuffles his or her library.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => a.OwningCard.AttachedTo == c));

            p.Effect = () => new SearchLibraryPutToZone(
              c => c.PutToBattlefield(),
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().Creature,
              text: "Search your library for a creature.",
              player: P(
                e => e.TriggerMessage<ZoneChanged>().Controller,
                evaluateOnResolve: true));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}