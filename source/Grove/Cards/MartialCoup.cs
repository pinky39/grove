namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Modifiers;

  public class MartialCoup : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Martial Coup")
        .ManaCost("{W}{W}").XCalculator((players, source, _) =>
          {
            var you = source.Controller;
            var maxX = you.ConvertedMana - source.ManaCost.Converted;

            if (maxX >= 5)
            {
              var yourScore = you.Battlefield.Creatures.Sum(x => x.Score);
              var opponentScore = players.GetOpponent(you).Battlefield.Creatures.Sum(x => x.Score);

              return opponentScore >= yourScore ? maxX : 4;
            }

            return maxX;
          })
        .Type("Sorcery")
        .Text(
          "Put X 1/1 white Soldier creature tokens onto the battlefield. If X is 5 or more, destroy all other creatures.")
        .FlavorText("Their war forgotten, the nations of Bant stood united in the face of a common threat.")
        .Effect<CreateTokens>((e, c) =>
          {
            e.Count = Value.PlusX;
            e.Tokens(c.Card
              .Named("Soldier Token")
              .FlavorText(
                "If you need an example to lead others to the front lines, consider the precedent set.")
              .Power(1)
              .Toughness(1)
              .Type("Creature Token Soldier")
              .Colors(ManaColors.White));

            e.BeforeResolve = (e1) =>
              {
                if (e1.X >= 5)
                {
                  e.Players.DestroyPermanents(
                    (permanent) => permanent.Is().Creature);
                }
              };
          });
    }
  }
}