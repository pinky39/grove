namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class BitterRevelation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bitter Revelation")
        .ManaCost("{3}{B}")
        .Type("Sorcery")
        .Text("Look at the top four cards of your library. Put two of them into your hand and the rest into your graveyard.{EOL}You lose 2 life.")
        .FlavorText("\"Here you lie then, Ugin. The corpses of worlds will join you in the tomb.\"{EOL}—Sorin Markov")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new LookAtTopCardsPutPartInHandRestIntoGraveyard(4, toHandAmount: 2),
            new ChangeLife(-2, P(e => e.Controller)));
        });
    }
  }
}
