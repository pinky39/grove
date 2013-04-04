namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Exhume : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Exhume")
        .ManaCost("{1}{B}")
        .Type("Sorcery")
        .Text("Each player puts a creature card from his or her graveyard onto the battlefield.")
        .FlavorText("Death—an outmoded concept. We sleep, and we change.")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerReturnsCardFromGraveyardToBattlefield();
            p.TimingRule(new ControllerGravayardCountIs(1, selector: c => c.Is().Creature));
          });
    }
  }
}