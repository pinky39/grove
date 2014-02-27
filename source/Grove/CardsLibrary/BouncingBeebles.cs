namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class BouncingBeebles : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bouncing Beebles")
        .ManaCost("{2}{U}")
        .Type("Creature Beeble")
        .Text("Bouncing Beebles can't be blocked as long as defending player controls an artifact.")
        .FlavorText(
          "Beebles are frequently hurled against stone surfaces at high speed but always zing back into the air with a giggle.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.UnblockableIfDedenderHasArtifacts);
    }
  }
}