namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class Rancor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Rancor")
        .ManaCost("{G}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature gets +2/+0 and has trample.{EOL}When Rancor is put into a graveyard from the battlefield, return Rancor to its owner's hand.")
        .FlavorText("Hatred outlives the hateful.")
        .Effect<Attach>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddPowerAndToughness>((m, _) => m.Power = 2),
          p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Trample)))
        .Timing(Timings.FirstMain())
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: C.Validator(Validators.EnchantedCreature()))
        .Abilities(
          C.TriggeredAbility(
            "When Rancor is put into a graveyard from the battlefield, return Rancor to its owner's hand.",
            C.Trigger<OnZoneChange>((t, _) =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            C.Effect<ReturnToHand>(e => e.ReturnOwner = true)));
    }
  }
}