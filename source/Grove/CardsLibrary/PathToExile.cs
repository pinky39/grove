namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class PathToExile : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Path to Exile")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Exile target creature. Its controller may search his or her library for a basic land card, put that card onto the battlefield tapped, then shuffle his or her library.")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new ExileTargets(),
            new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              afterPutToZone: (c, g) => c.Tap(),
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().BasicLand,
              text: "Search your library for a basic land card.",
              player: P(e => e.Target.Controller())));

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectExileBattlefield());
        });
    }
  }
}
