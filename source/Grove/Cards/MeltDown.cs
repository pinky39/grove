namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

  public class MeltDown : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Meltdown")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Destroy each artifact with converted mana cost X or less.")
        .FlavorText(
          "Catastrophes happened so often at the mana rig that the viashino language had a special word to describe them.")
        .Cast(p =>
          {
            p.XCalculator = ChooseXAi.DestroyEach((card, x) => 
              card.Is().Artifact && card.ConvertedCost <= x);
            
            p.Timing = Timings.FirstMain();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyAllPermanents>(e => e.Filter = 
              (self, card) => card.Is().Artifact && card.ConvertedCost <= self.X);
          });
    }
  }
}