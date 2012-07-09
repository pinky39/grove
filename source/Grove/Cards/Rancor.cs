namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;
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
        .Effect<EnchantCreature>((e, c) => e.Modifiers(
          c.Modifier<AddPowerAndToughness>((m, _) => m.Power = 2),
          c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Trample)))
        .Timing(Timings.FirstMain())
        .Targets(
          filter: TargetFilters.CombatEnchantment(),
          selectors: C.Selector(Selectors.EnchantedCreature()))
        .Abilities(
          C.TriggeredAbility(
            "When Rancor is put into a graveyard from the battlefield, return Rancor to its owner's hand.",
            C.Trigger<ChangeZone>((t, _) =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            C.Effect<ReturnToOwnersHand>()));
    }
  }
}