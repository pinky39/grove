namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class ElvishLyrist : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Elvish Lyrist")
        .ManaCost("{G}")
        .Type("Creature Elf")
        .Text("{G},{T}, Sacrifice Elvish Lyrist: Destroy target enchantment.")
        .FlavorText(
          "Bring the spear of ancient briar;{EOL}Bring the torch to light the pyre.{EOL}Bring the one who trod our ground;{EOL}Bring the spade to dig his mound.")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "{G},{T}, Sacrifice Elvish Lyrist: Destroy target enchantment.",
            Cost<PayMana, Tap>(cost => cost.Amount = "{G}".ParseMana()),
            Effect<DestroyTargetPermanents>(),
            timing: Timings.InstantRemovalTarget(),
            effectValidator: TargetValidator(TargetIs.Card(card => card.Is().Enchantment), ZoneIs.Battlefield()),
            targetSelectorAi: TargetSelectorAi.OrderByDescendingScore()            
            )
        );
    }
  }
}