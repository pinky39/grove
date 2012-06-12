namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class SignInBlood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sign in Blood")
        .ManaCost("{B}{B}")
        .Type("Sorcery")
        .Text("Target player draws two cards and loses 2 life.")
        .FlavorText(
          "'You know I accept only one currency here, and yet you have sought me out. Why now do you hesitate?'{EOL}—Xathrid demon")
        .Timing(Timings.Steps(Step.FirstMain))
        .Effect<TargetPlayerDrawsCards>((e, _) =>
        {
          e.CardCount = 2;
          e.LifeLoss = 2;
        })
        .Target(C.Selector(target => target.IsPlayer()));   
    }
  }
}