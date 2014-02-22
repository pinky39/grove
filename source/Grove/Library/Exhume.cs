namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

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
            p.Effect = () => new EachPlayerPutsCardToBattlefield(Zone.Graveyard, c => c.Is().Creature);
            p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}