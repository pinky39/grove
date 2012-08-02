namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class DiabolicServitude : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Diabolic Servitude")
        .ManaCost("{3}{B}")
        .Type("Enchantment")
        .Text(
          "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.{EOL}When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.{EOL}When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.")
        .Timing(Timings.MainPhases())
        .Abilities(
          C.TriggeredAbility(
            "When Diabolic Servitude enters the battlefield, return target creature card from your graveyard to the battlefield.",
            C.Trigger<ChangeZone>(t => t.To = Zone.Battlefield),
            C.Effect<CompoundEffect>(p => p.Effect.ChildEffects(
              p.Builder.Effect<PutTargetToBattlefield>(),
              p.Builder.Effect<ApplyModifiersToTargets>(p1 => p1.Effect.Modifiers(
                p1.Builder.Modifier<AddTriggeredAbility>((m, c) => m.Ability = c.TriggeredAbility(
                  "When the creature put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.",
                  c.Trigger<ChangeZone>(t =>
                    {
                      t.From = Zone.Battlefield;
                      t.To = Zone.Graveyard;
                    }),
                  c.Effect<CompoundEffect>(p2 => p2.Effect.ChildEffects(
                    p2.Builder.Effect<ExileOwner>(),
                    p2.Builder.Effect<ReturnToHand>(e => e.ReturnCard = e.Source.SourceCard)
                    ))
                  )),
                p1.Builder.Modifier<AddTriggeredAbility>((m, c) => m.Ability = c.TriggeredAbility(
                  "When Diabolic Servitude leaves the battlefield, exile the creature put onto the battlefield with Diabolic Servitude.",
                  c.Trigger<ChangeZone>(t =>
                    {
                      t.From = Zone.Battlefield;
                      t.Filter = (ability, card) => card == ability.SourceCard;
                    }),
                  c.Effect<ExileOwner>()
                  ))
                ))
              )),
            selectorAi: TargetSelectorAi.OrderByDescendingScore(),
            effectValidator: C.Validator(Validators.CardInGraveyard(card => card.Is().Creature))));
    }
  }
}