namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class TreasureCruise : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treasure Cruise")
        .ManaCost("{7}{U}")
        .Type("Sorcery")
        .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}Draw three cards.")
        .FlavorText("Countless delights drift on the surface while dark schemes run below.")
        .Cast(p =>
        {
          p.Effect = () => new DrawCards(3);
          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
