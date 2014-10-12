namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ConstrictingSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Constricting Sliver")
        .ManaCost("{5}{W}")
        .Type("Creature — Sliver")
        .Text(
          "Sliver creatures you control have \"When this creature enters the battlefield, you may exile target creature an opponent controls until this creature leaves the battlefield.\"")
        .FlavorText(
          "Slivers are often seen toying with enemies they capture, not out of cruelty, but to fully learn their physical capabilities.")
        .Power(3)
        .Toughness(3)
        .ContinuousEffect(p =>
          {
            p.CardFilter =
              (card, effect) =>
                card.Controller == effect.Source.Controller && card.Is().Creature &&
                  card.Is("sliver");
            p.Modifier = () =>
              {
                var tp = new TriggeredAbility.Parameters();

                tp.Text =
                  "When this creature enters the battlefield, you may exile target creature an opponent controls until this creature leaves the battlefield.";
                tp.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                tp.Effect = () => new CompoundEffect(
                  new ExileTargets(),
                  new Attach());

                tp.TargetSelector.AddEffect(
                  trg => trg.Is.Card(c => c.Is().Creature, controlledBy: ControlledBy.Any).On.Battlefield());

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              };
            p.ApplyOnlyToPermaments = false;
          })
        .ContinuousEffect(p =>
          {
            p.CardFilter =
              (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature && card.Is("sliver");
            p.Modifier = () =>
              {
                var tp = new TriggeredAbility.Parameters();

                tp.Text =
                  "When this creature enters the battlefield, you may exile target creature an opponent controls until this creature leaves the battlefield.";
                tp.Trigger(new OnZoneChanged(
                  from: Zone.Battlefield,
                  filter: (c, a, g) => a.OwningCard == c && a.OwningCard.AttachedTo != null));

                tp.Effect = () => new PutIntoPlay(P(e => e.Source.OwningCard.AttachedTo));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              };
            p.ApplyOnlyToPermaments = false;
          });
    }
  }
}