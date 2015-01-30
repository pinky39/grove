namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TimingRules;
  using Effects;
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
              .Colors(CardColor.Black, CardColor.Green)
              .Power(0)
              .Toughness(0)
              .StaticAbility(sp =>
              {
                sp.Modifier(() => new AddPowerAndToughness(
                  getPower: (player) => player.Battlefield.Creatures.Max(c => c.Toughness.GetValueOrDefault()),
                  getToughness: (player) => player.Battlefield.Creatures.Max(c => c.Toughness.GetValueOrDefault())
                  ));
                sp.EnabledInAllZones = false;
              }));
        });
    }
  }
}
