namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class BeastOfBurden : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Beast of Burden")
        .ManaCost("{6}")
        .Type("Artifact Creature Golem")
        .Text("Beast of Burden's power and toughness are each equal to the number of creatures on the battlefield.")
        .FlavorText(
          "'If it is meant to be nothing but a machine,' Karn finally asked Jhoira, 'why did Urza build it to be like me?'")
        .Power(0)
        .Toughness(0)
        .StaticAbility(p =>
          {
            p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
              power: 1,
              toughness:1,
              filter: (c, _) => c.Is().Creature,
              modifier: () => new IntegerSetter(),
              controlledBy: ControlledBy.Any));
            
            p.EnabledInAllZones = true;
          });
    }
  }
}