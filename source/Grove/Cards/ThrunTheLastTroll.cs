namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class ThrunTheLastTroll : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Thrun, the Last Troll")
        .ManaCost("{2}{G}{G}")
        .Type("Legendary Creature - Troll Shaman")
        .Text(
          "Thrun can't be countered.{EOL}Thrun can't be the target of spells or abilities your opponents control.{EOL}{1}{G}: Regenerate Thrun.")
        .FlavorText("His crime was silence, and now he suffers it eternally.")
        .Power(4)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Effect<PutIntoPlay>(e => e.CanBeCountered = false)
        .Abilities(
          Static.Hexproof,
          ActivatedAbility(
            "{1}{G}: Regenerate Thrun.",
            Cost<PayMana>(c => c.Amount = "{1}{G}".ParseMana()),
            Effect<Regenerate>(),
            timing: Timings.Regenerate()));
    }
  }
}