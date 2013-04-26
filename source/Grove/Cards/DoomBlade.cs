namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class DoomBlade : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Doom blade")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonblack creature.")
        .FlavorText("The void is without substance but cuts like steel.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black))
              .On.Battlefield());

            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}