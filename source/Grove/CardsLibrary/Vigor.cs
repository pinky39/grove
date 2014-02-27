namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class Vigor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vigor")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature - Elemental Incarnation")
        .Text(
          "{Trample}{EOL}If damage would be dealt to a creature you control other than Vigor, prevent that damage. Put a +1/+1 counter on that creature for each 1 damage prevented this way.{EOL}When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Trample)
        .StaticAbility(p => p.Modifier(() => new AddDamagePrevention(modifier => new ReplaceDamageToPlayersCreaturesWithCounters(
            player: modifier.SourceCard.Controller, 
            counter: () => new PowerToughness(1, 1),
            filter: card => card.Name != "Vigor"))))                
        .TriggeredAbility(p =>
          {
            p.Text = "When Vigor is put into a graveyard from anywhere, shuffle it into its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.Graveyard));
            p.Effect = () => new ShuffleOwningCardIntoLibrary();
          });
    }
  }
}