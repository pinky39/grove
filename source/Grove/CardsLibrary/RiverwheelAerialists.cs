namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class RiverwheelAerialists : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Riverwheel Aerialists")
        .ManaCost("{5}{U}")
        .Type("Creature — Djinn Monk")
        .Text("{Flying}{EOL}{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}")
        .FlavorText("Adepts of the Riverwheel Stronghold can run through rain and never get wet; masters use the raindrops as stepping stones.")
        .Power(4)
        .Toughness(5)
        .Prowess()
        .SimpleAbilities(Static.Flying);
    }
  }
}
