namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Player;
  using Gameplay.Zones;

  public class DiabolicServitude : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Diabolic Servitude")
        .ManaCost("{3}{B}")
        .Type("Enchantment")
        .Text(
          "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.{EOL}When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.{EOL}When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.")
        .Cast(p => p.TimingRule(new ControllerGravayardCountIs(1, selector: c => c.Is().Creature)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CompoundEffect(
              new PutTargetsToBattlefield(),
              new Attach());

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().In.YourGraveyard());
            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.SpellOwner));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (ability, card) => ability.SourceCard.AttachedTo == card));

            p.Effect = () => new CompoundEffect(
              new ExileCard(P(e => e.Source.OwningCard.AttachedTo)),
              new ReturnToHand(returnOwningCard: true));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              filter: (ability, card) => ability.OwningCard == card && ability.OwningCard.AttachedTo != null));

            p.Effect = () => new ExileCard(P(e => e.Source.OwningCard.AttachedTo));
          }
        );
    }
  }
}