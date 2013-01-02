namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class AlbinoTroll : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Albino Troll")
        .ManaCost("{1}{G}")
        .Type("Creature Troll")
        .Text(
          "{Echo} {1}{G} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}{1}{G}: Regenerate Albino Troll.")
        .Power(3)
        .Toughness(3)
        .Echo("{1}{G}")
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "{1}{G}: Regenerate Albino Troll.",
            Cost<PayMana>(c => c.Amount = "{1}{G}".ParseMana()),
            Effect<Regenerate>(),
            timing: Timings.Regenerate(),
            category: EffectCategories.Protector));
    }
  }
}