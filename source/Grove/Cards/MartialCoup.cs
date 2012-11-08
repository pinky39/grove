namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class MartialCoup : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Martial Coup")
        .ManaCost("{W}{W}").XCalculator(p =>
          {
            var you = p.Controller;
            var maxX = you.GetConvertedMana(ManaUsage.Spells) - p.Source.ManaCost.Converted;

            if (maxX >= 5)
            {
              var yourScore = you.Battlefield.Creatures.Sum(x => x.Score);
              var opponentScore = p.Opponent.Battlefield.Creatures.Sum(x => x.Score);

              return opponentScore >= yourScore ? maxX : 4;
            }

            return maxX;
          })
        .Type("Sorcery")
        .Text(
          "Put X 1/1 white Soldier creature tokens onto the battlefield. If X is 5 or more, destroy all other creatures.")
        .FlavorText("Their war forgotten, the nations of Bant stood united in the face of a common threat.")
        .Category(EffectCategories.Destruction)
        .Timing(Timings.MainPhases())
        .Effect<CreateTokens>(e =>
          {
            e.Count = Value.PlusX;
            e.Tokens(Card
              .Named("Soldier Token")
              .FlavorText(
                "If you need an example to lead others to the front lines, consider the precedent set.")
              .Power(1)
              .Toughness(1)
              .Type("Creature Token Soldier")
              .Colors(ManaColors.White));

            e.BeforeResolve = self =>
              {
                if (self.X >= 5)
                {
                  self.Players.DestroyPermanents(
                    (permanent) => permanent.Is().Creature);
                }
              };
          });
    }
  }
}