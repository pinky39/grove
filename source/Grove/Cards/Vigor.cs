namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Damage;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.Zones;

  public class Vigor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Vigor")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Trample}{EOL}If damage would be dealt to a creature you control other than Vigor, prevent that damage. Put a +1/+1 counter on that creature for each 1 damage prevented this way.{EOL}When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .StaticAbilities(Static.Trample)
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddDamagePrevention(new ReplaceDamageWithCounters(() => new PowerToughness(1, 1)));
            p.CardFilter = (card, effect) =>
              card.Name != effect.Source.Name && card.Controller == effect.Source.Controller && card.Is().Creature;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.Graveyard));
            p.Effect = () => new ShuffleIntoLibrary();
          });
    }
  }
}