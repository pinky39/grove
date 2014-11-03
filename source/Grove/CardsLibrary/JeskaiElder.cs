namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class JeskaiElder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jeskai Elder")
        .ManaCost("{1}{U}")
        .Type("Creature — Human Monk")
        .Text("{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}{EOL}Whenever Jeskai Elder deals combat damage to a player, you may draw a card. If you do, discard a card.")
        .Power(1)
        .Toughness(2)
        .Prowess()
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Jeskai Elder deals combat damage to a player, you may draw a card. If you do, discard a card.";
          p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat && dmg.IsDealtByOwningCard && dmg.IsDealtToPlayer));

          p.Effect = () => new DrawCards(1, discardCount: 1);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
