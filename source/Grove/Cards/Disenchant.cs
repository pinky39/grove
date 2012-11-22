namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;  
  
  public class Disenchant : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Disenchant")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target artifact or enchantment.")
        .FlavorText("'Let Phyrexia breed evil in the darkness; my holy light will reveal its taint.{EOL}—Serra")
        .Effect<DestroyTargetPermanents>()
        .Category(EffectCategories.Destruction)
        .Timing(Timings.InstantRemovalTarget())
        .Targets(
          TargetSelectorAi.OrderByDescendingScore(),          
          TargetValidator(
            TargetIs.Card(card => card.Is().Artifact || card.Is().Enchantment),
            ZoneIs.Battlefield()));
    }
  }
}