namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class DiabolicServitude : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Diabolic Servitude")
        .ManaCost("{3}{B}")
        .Type("Enchantment")
        .Text(
          "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.{EOL}When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.{EOL}When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.")
        .Cast(p => p.Timing = All(Timings.MainPhases(), Timings.HasCardsInGraveyard(card => card.Is().Creature)))                
        .Abilities(
          TriggeredAbility(
            "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<PutTargetsToBattlefield>(),
              Effect<Attach>())),
            selectorAi: TargetSelectorAi.OrderByScore(Controller.SpellOwner),
            effectValidator: Target(
              Validators.Card(card => card.Is().Creature),
              Zones.YourGraveyard())),
          TriggeredAbility(
            "When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.",
            Trigger<OnZoneChange>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
                t.Filter = (ability, card) => ability.SourceCard.AttachedTo == card;
              }),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<ExileCard>(c => { c.Card = c.Source.OwningCard.AttachedTo; }),
              Effect<PutToHand>(c => c.Card = c.Source.OwningCard)))
            ),
          TriggeredAbility(
            "When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.",
            Trigger<OnZoneChange>(t =>
              {
                t.From = Zone.Battlefield;
                t.Filter = (ability, card) => ability.OwningCard == card && ability.OwningCard.AttachedTo != null;
              }),
            Effect<ExileCard>(e =>
              {
                e.Card = e.Source.OwningCard.AttachedTo;
              })
            ));
    }
  }
}