namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class HypnoticSpecter : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hypnotic Specter")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Specter")
        .Text(
          "{Flying}{EOL}Whenever Hypnotic Specter deals damage to an opponent, that player discards a card at random.")
        .FlavorText("'Its victims are known by their eyes shattered vessels leaking broken dreams.")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Hypnotic Specter deals damage to an opponent, that player discards a card at random.";

            p.Trigger(new OnDamageDealt(
              playerFilter: (player, t, _) => player != t.OwningCard.Controller));

            p.Effect = () => new OpponentDiscardsCards(randomCount: 1);
          });
    }
  }
}