namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class UnworthyDead : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Unworthy Dead")
        .ManaCost("{1}{B}")
        .Type("Creature Skeleton")
        .Text("{B}: Regenerate Unworthy Dead.")
        .FlavorText(
          "'Great Yawgmoth moves across the seas of shard and bone and rust. We exalt him in life, in death, and in between.'{EOL}—Phyrexian Scriptures")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{B}: Regenerate Unworthy Dead.",
            Cost<PayMana>(c => c.Amount = "{B}".ParseMana()),
            Effect<Regenerate>(),
            timing: Timings.Regenerate(),
            category: EffectCategories.Protector));
    }
  }
}