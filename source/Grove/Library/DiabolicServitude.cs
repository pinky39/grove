namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

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
            p.TargetingRule(new EffectRankBy(c => -c.Score));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => a.SourceCard.AttachedTo == c));

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
              filter: (c, a, g) => a.OwningCard == c && a.OwningCard.AttachedTo != null));

            p.Effect = () => new ExileCard(P(e => e.Source.OwningCard.AttachedTo), Zone.Battlefield);
          }
        );
    }
  }
}