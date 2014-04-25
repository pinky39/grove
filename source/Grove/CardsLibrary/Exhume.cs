namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Exhume : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Exhume")
        .ManaCost("{1}{B}")
        .Type("Sorcery")
        .Text("Each player puts a creature card from his or her graveyard onto the battlefield.")
        .FlavorText("Death—an outmoded concept. We sleep, and we change.")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerPutsACardToBattlefield(Zone.Graveyard, c => c.Is().Creature);
            p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}