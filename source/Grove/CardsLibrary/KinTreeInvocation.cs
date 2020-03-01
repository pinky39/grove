namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Grove.AI.TimingRules;
  using Modifiers;

  public class KinTreeInvocation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kin-Tree Invocation")
        .ManaCost("{B}{G}")
        .Type("Sorcery")
        .Text("Put an X/X black and green Spirit Warrior creature token onto the battlefield, where X is the greatest toughness among creatures you control.")
        .FlavorText("The passing years add new rings to the tree's trunk, bolstering the spirits that dwell within.")        
        .Cast(p =>
        {
          p.Effect = () => new CreateTokens(
            count: 1,
            token: Card
              .Named("Spirit Warrior")
              .Type("Token Creature - Spirit Warrior")
              .Colors(CardColor.Black, CardColor.Green),
            tokenParameters: (token, ctx) =>
            {
              token.Power(ctx.You.Battlefield.Creatures.Max(c => c.Toughness.GetValueOrDefault()));
              token.Toughness(ctx.You.Battlefield.Creatures.Max(c => c.Toughness.GetValueOrDefault()));
            });

          p.TimingRule(new WhenYouHavePermanents(c => c.Toughness.GetValueOrDefault() >= 2));
        });
    }
  }
}
