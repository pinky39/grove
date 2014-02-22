namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class Reprocess : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Reprocess")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text(
          "Sacrifice any number of artifacts, creatures, and/or lands. Draw a card for each permanent sacrificed this way.")
        .FlavorText("Everything will find its use in Phyrexia. Eventually.")
        .Cast(p =>
          {
            p.Effect = () => new DrawCardsEqualToSacrificedPermanentsCount(
              text: "Sacrifice any number of artifacts, creatures, and/or lands.",
              validator: c => c.Is().Land || c.Is().Creature || c.Is().Artifact);

            p.TimingRule(new OnSecondMain());
          });
    }
  }
}