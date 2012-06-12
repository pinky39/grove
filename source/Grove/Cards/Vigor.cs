namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Counters;
  using Core.DamagePrevention;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;
  using Core.Zones;

  public class Vigor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Vigor")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Trample}{EOL}If damage would be dealt to a creature you control other than Vigor, prevent that damage. Put a +1/+1 counter on that creature for each 1 damage prevented this way.{EOL}When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .Abilities(
          StaticAbility.Trample,
          C.Continuous((e, c) => {
            e.ModifierFactory = c.Modifier<AddDamagePrevention>((m, c0) =>
              m.Kind = c0.Prevention<ReplaceDamageWithCounters>(
                (p, c1) => p.CounterFactory = c1.Counter<PowerToughness>((counter, _) => {
                  counter.Power = 1;
                  counter.Toughness = 1;
                })));
            e.Filter = (card, vigor) => card.Name != vigor.Name && card.Controller == vigor.Controller;
          }),
          C.TriggeredAbility(
            "When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Graveyard),
            C.Effect<ShuffleIntoLibrary>()));
    }
  }
}