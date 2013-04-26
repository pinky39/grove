namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.CostRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class MeltDown : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Meltdown")
        .ManaCost("{R}").HasXInCost()
        .Type("Sorcery")
        .Text("Destroy each artifact with converted mana cost X or less.")
        .FlavorText(
          "Catastrophes happened so often at the mana rig that the viashino language had a special word to describe them.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllPermanents(
              (e, card) => card.Is().Artifact && card.ConvertedCost <= e.X);

            p.TimingRule(new FirstMain());

            p.CostRule(new DestroyEachPermanent((card, x) =>
              card.Is().Artifact && card.ConvertedCost <= x));
          });
    }
  }
}