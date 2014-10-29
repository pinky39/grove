namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class ScouredBarrens : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scoured Barrens")
        .Type("Land")
        .Text("Scoured Barrens enters the battlefield tapped.{EOL}When Scoured Barrens enters the battlefield, you gain 1 life.{EOL}{T}: Add {W} or {B} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {W} or {B} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isWhite: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Scoured Barrens enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, yours: true);
        });
    }
  }
}
