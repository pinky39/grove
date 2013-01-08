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

  public class TrollAscetic : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Troll Ascetic")
        .ManaCost("{1}{G}{G}")
        .Type("Creature - Troll Shaman")
        .Text(
          "{Hexproof}{EOL}{1}{G}: Regenerate Troll Ascetic.")
        .FlavorText("It's no coincidence that the oldest trolls are also the angriest.")
        .Power(3)
        .Toughness(2)        
        .Abilities(
          Static.Hexproof,
          ActivatedAbility(
            "{1}{G}: Regenerate Troll Ascetic.",
            Cost<PayMana>(cost => cost.Amount = "{1}{G}".ParseMana()),
            Effect<Regenerate>(),
            timing: Timings.Regenerate()));
    }
  }
}