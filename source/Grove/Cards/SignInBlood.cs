namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class SignInBlood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sign in Blood")
        .ManaCost("{B}{B}")
        .Type("Sorcery")
        .Text("Target player draws two cards and loses 2 life.")
        .FlavorText(
          "'You know I accept only one currency here, and yet you have sought me out. Why now do you hesitate?'{EOL}—Xathrid demon")
        .Timing(Timings.FirstMain())
        .Effect<TargetPlayerDrawsCards>(e =>
          {
            e.CardCount = 2;
            e.LifeLoss = 2;
          })
        .Targets(
          selectorAi: TargetSelectorAi.Controller(),
          effectValidator: TargetValidator(TargetIs.Player(), ZoneIs.None()));
    }
  }
}