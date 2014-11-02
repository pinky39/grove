namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SuspensionField : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Suspension Field")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("When Suspension Field enters the battlefield, you may exile target creature with toughness 3 or greater until Suspension Field leaves the battlefield. {I}(That creature returns under its owner's control.){/I}")
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new CompoundEffect(
            new ExileTargets(),
            new Attach());

          p.TargetSelector.AddEffect(
            trg => trg.Is.Card(c => c.Is().Creature && c.Power >= 3).On.Battlefield());
        })
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(
                  from: Zone.Battlefield,
                  filter: (c, a, g) => a.OwningCard == c && a.OwningCard.AttachedTo != null));

          p.Effect = () => new PutCardToBattlefield(P(e => e.Source.OwningCard.AttachedTo), Zone.Exile);
        });
    }
  }
}
