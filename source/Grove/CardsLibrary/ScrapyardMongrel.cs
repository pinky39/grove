namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class ScrapyardMongrel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scrapyard Mongrel")
        .ManaCost("{3}{R}")
        .Type("Creature — Hound")
        .Text(
          "As long as you control an artifact, Scrapyard Mongrel gets +2/+0 and has trample.{I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}")
        .FlavorText("Trespassers are welcome to try.")
        .Power(3)
        .Toughness(3)
        .StaticAbility(p =>
          {                        
            p.Modifier(() => new AddSimpleAbility(Static.Trample));
            p.Modifier(() => new AddPowerAndToughness(2, 0));
            p.Condition = cond => cond.OwnerControlsPermanent(c => c.Is().Artifact);
          });
    }
  }
}