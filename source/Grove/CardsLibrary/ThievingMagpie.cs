namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class ThievingMagpie : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thieving Magpie")
        .ManaCost("{2}{U}{U}")
        .Type("Creature Bird")
        .Text("{Flying}{EOL}Whenever Thieving Magpie deals damage to an opponent, draw a card.")
        .Power(1)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Thieving Magpie deals damage to an opponent, draw a card.";

            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsDealtByOwningCard &&
                dmg.IsDealtToOpponent));
                            
            p.Effect = () => new DrawCards(1);
          });
    }
  }
}