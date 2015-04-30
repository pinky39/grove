namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class NecrogenScudder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Necrogen Scudder")
        .ManaCost("{2}{B}")
        .Type("Creature — Horror")
        .Text("{Flying}{EOL}When Necrogen Scudder enters the battlefield, you lose 3 life.")
        .FlavorText("Contrary to popular belief, it's kept aloft by necrogen gas, not the screaming agony of a thousand murdered souls.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
        {
          p.Text = "When Necrogen Scudder enters the battlefield, you lose 3 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(amount: -3, whos: P(e => e.Controller));
        });
    }
  }
}
