namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;

  public class TrollAscetic : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Troll Ascetic")
        .ManaCost("{1}{G}{G}")
        .Type("Creature - Troll Shaman")
        .Text(
          "{Hexproof}{EOL}{1}{G}: Regenerate Troll Ascetic.")
        .FlavorText("It's no coincidence that the oldest trolls are also the angriest.")
        .Power(3)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Hexproof,
          C.ActivatedAbility(
            "{1}{G}: Regenerate Troll Ascetic.",
            C.Cost<TapOwnerPayMana>((c, _) => c.Amount = "{1}{G}".ParseManaAmount()),
            C.Effect<Regenerate>(),
            timing: Timings.Regenerate()));
    }
  }
}