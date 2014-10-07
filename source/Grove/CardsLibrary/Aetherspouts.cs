namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Aetherspouts : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("AEtherspouts")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("For each attacking creature, its owner puts it on the top or bottom of his or her library.")
        .FlavorText("\"Don't worry, there's plenty for everyone.\"{EOL}—Vickon, Eleventh Company battlemage ")
        .Cast(p =>
        {
          p.Text = "For each attacking creature, its owner puts it on the top or bottom of his or her library.";

          p.Effect = () => new PutSelectedAttackersOnTopRestOnBottom();

          p.TimingRule(new AfterOpponentDeclaresAttackers());
        });
    }
  }
}
