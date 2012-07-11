namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;

  public class UnworthyDead : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Unworthy Dead")
        .ManaCost("{1}{B}")
        .Type("Creature Skeleton")
        .Text("{B}: Regenerate Unworthy Dead.")
        .FlavorText(
          "'Great Yawgmoth moves across the seas of shard and bone and rust. We exalt him in life, in death, and in between.'{EOL}—Phyrexian Scriptures")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{B}: Regenerate Unworthy Dead.",
            C.Cost<TapOwnerPayMana>((c, _) => c.Amount = "{B}".ParseManaAmount()),
            C.Effect<Regenerate>(),
            timing: Timings.Regenerate(),
            category: EffectCategories.Protector));
    }
  }
}