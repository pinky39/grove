namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class HorizonChimera : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Horizon Chimera")
        .ManaCost("{2}{G}{U}")
        .Type("Creature — Chimera")
        .Text(
          "{Flash}{I}(You may cast this spell any time you could cast an instant.){/I}{EOL}{Flying}, {trample}{EOL}Whenever you draw a card, you gain 1 life.")
        .Power(3)
        .Toughness(2)
        .SimpleAbilities(Static.Flash, Static.Flying, Static.Trample)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you draw a card, you gain 1 life.";

          p.Trigger(new OnPlayerDrawsCard((ability, player) => ability.OwningCard.Controller == player));

          p.Effect = () => new ChangeLife(amount: 1, yours: true);
        });
    }
  }
}
