namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class BlindingSpray : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blinding Spray")
        .ManaCost("{4}{U}")
        .Type("Instant")
        .Text("Creatures your opponents control get -4/-0 until end of turn.{EOL}Draw a card.")
        .FlavorText("\"The stronger our enemies seem, the more vulnerable they are.\"{EOL}—Sultai secret")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Creature && c.Controller == ctx.Opponent,
              modifier: () => new AddPowerAndToughness(-4, 0) { UntilEot = true }),
            new DrawCards(1));

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new AfterOpponentDeclaresBlockers()));
        });
    }
  }
}
