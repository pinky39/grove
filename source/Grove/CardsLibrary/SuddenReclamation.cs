namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class SuddenReclamation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sudden Reclamation")
        .ManaCost("{3}{G}")
        .Type("Instant")
        .Text("Put the top four cards of your library into your graveyard, then return a creature card and a land card from your graveyard to your hand.")
        .FlavorText("\"There is no secret buried so deep that we will never find it.\"{EOL}—Kurtar, Sultai necromancer")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new PlayerPutsTopCardsFromLibraryToGraveyard(P(e => e.Controller), count: 4),
            new ReturnToHand());
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.YourGraveyard());
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.YourGraveyard());
          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
        });
    }
  }
}
