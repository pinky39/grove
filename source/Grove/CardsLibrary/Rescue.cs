namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class Rescue : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rescue")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Return target permanent you control to its owner's hand.")
        .FlavorText(
          "Urza, I've discovered more promising students than you and my husband have, combined. And I haven't lost a single one.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnToHand();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(controlledBy: ControlledBy.SpellOwner)
              .On.Battlefield());

            p.TargetingRule(new EffectProtect());
          });
    }
  }
}