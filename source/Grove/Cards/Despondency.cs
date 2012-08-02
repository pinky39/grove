namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class Despondency : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Despondency")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets -2/-0.{EOL}When Despondency is put into a graveyard from the battlefield, return Despondency to its owner's hand.")
        .Timing(Timings.FirstMain())
        .Targets(
          selectorAi: TargetSelectorAi.ReducePower(2),
          effectValidator: C.Validator(Validators.EnchantedCreature()))
        .Effect<EnchantCreature>(
          p => p.Effect.Modifiers(p.Builder.Modifier<AddPowerAndToughness>((m, _) => m.Power = -2)))
        .Abilities(
          C.TriggeredAbility(
            "When Despondency is put into a graveyard from the battlefield, return Despondency to its owner's hand.",
            C.Trigger<ChangeZone>((t, _) =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            C.Effect<ReturnToHand>(e => e.ReturnOwner = true)));
    }
  }
}