namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class Vigor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Vigor")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Trample}{EOL}If damage would be dealt to a creature you control other than Vigor, prevent that damage. Put a +1/+1 counter on that creature for each 1 damage prevented this way.{EOL}When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Trample,
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddDamagePrevention>(m =>
                m.Prevention = Prevention<ReplaceDamageWithCounters>(
                  p => p.CounterFactory = Counter<PowerToughness>(counter =>
                    {
                      counter.Power = 1;
                      counter.Toughness = 1;
                    })));
              e.CardFilter = (card, vigor) => card.Name != vigor.Name && card.Controller == vigor.Controller && card.Is().Creature;
            }),
          TriggeredAbility(
            "When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.",
            Trigger<OnZoneChange>(t => t.To = Zone.Graveyard),
            Effect<ShuffleIntoLibrary>()));
    }
  }
}