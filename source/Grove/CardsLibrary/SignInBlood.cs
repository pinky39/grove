namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class SignInBlood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sign in Blood")
        .ManaCost("{B}{B}")
        .Type("Sorcery")
        .Text("Target player draws two cards and loses 2 life.")
        .FlavorText(
          "You know I accept only one currency here, and yet you have sought me out. Why now do you hesitate?")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerDrawsCards(2, lifeLoss: 2);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectYou());
          });
    }
  }
}