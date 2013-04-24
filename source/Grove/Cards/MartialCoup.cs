namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class MartialCoup : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Martial Coup")
        .ManaCost("{W}{W}").HasXInCost()
        .Type("Sorcery")
        .Text(
          "Put X 1/1 white Soldier creature tokens onto the battlefield. If X is 5 or more, destroy all other creatures.")
        .FlavorText("Their war forgotten, the nations of Bant stood united in the face of a common threat.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new DestroyAllPermanents(
                filter: (e, c) => c.Is().Creature)
                {ShouldResolve = e => e.X >= 5},
              new CreateTokens(
                count: Value.PlusX,
                token: Card
                  .Named("Soldier Token")
                  .FlavorText(
                    "If you need an example to lead others to the front lines, consider the precedent set.")
                  .Power(1)
                  .Toughness(1)
                  .Type("Creature Token Soldier")
                  .Colors(CardColor.White)));

            p.TimingRule(new SecondMain());
            p.CostRule(new Ai.CostRules.MartialCoup(5));
          });
    }
  }
}