namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class Dread : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Dread")
        .ManaCost("{3}{B}{B}{B}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Fear}{EOL}Whenever a creature deals damage to you, destroy it.{EOL}When Dread is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)        
        .Abilities(
          Static.Fear,
          Continuous(e =>
            {
              e.CardFilter = (card, source) => card.Is().Creature;
              e.ModifierFactory = Modifier<AddTriggeredAbility>(m =>
                m.Ability = TriggeredAbility(
                  "Whenever a creature deals damage to you, destroy it.",
                  Trigger<OnDamageDealt>(t => t.ToYou()),
                  Effect<DestroySource>(),
                  abilityCategory: EffectCategories.Destruction));
            }),
          TriggeredAbility(
            "When Dread is put into a graveyard from anywhere, shuffle it into its owner's library.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Graveyard),
            Effect<ShuffleIntoLibrary>())
        );
    }
  }
}