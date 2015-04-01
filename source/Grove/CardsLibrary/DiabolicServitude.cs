namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class DiabolicServitude : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Diabolic Servitude")
        .ManaCost("{3}{B}")
        .Type("Enchantment")
        .Text(
          "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.{EOL}When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.{EOL}When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.")
        .Cast(p => p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Creature)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CompoundEffect(
              new PutTargetsToBattlefield(),
              new Attach());

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().In.YourGraveyard());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              selector: (c, ctx) => ctx.OwningCard.AttachedTo == c));

            p.Effect = () => new CompoundEffect(
              new ExileCard(P(e => e.Source.OwningCard.AttachedTo), Zone.Graveyard),
              new ReturnToHand(returnOwningCard: true));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              selector: (c, ctx) => ctx.OwningCard == c && ctx.OwningCard.AttachedTo != null));

            p.Effect = () => new ExileCard(P(e => e.Source.OwningCard.AttachedTo), Zone.Battlefield);
          }
        );
    }
  }
}