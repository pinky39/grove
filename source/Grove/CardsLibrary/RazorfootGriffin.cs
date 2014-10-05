namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class RazorfootGriffin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Razorfoot Griffin")
        .ManaCost("{3}{W}")
        .Type("Creature — Griffin")
        .Text("{Flying} {I}(This creature can't be blocked except by creatures with flying or reach.){/I}{EOL}{First strike} {I}(This creature deals combat damage before creatures without first strike.){/I}")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.FirstStrike, Static.Flying);
    }
  }
}
