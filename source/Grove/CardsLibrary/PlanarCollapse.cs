namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Effects;
  using Grove.Triggers;

  public class PlanarCollapse : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Planar Collapse")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, if there are four or more creatures on the battlefield, sacrifice Planar Collapse and destroy all creatures. They can't be regenerated.")
        .FlavorText("With heavy heart, Urza doused one world's light to rekindle another's.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if there are four or more creatures on the battlefield, sacrifice Planar Collapse and destroy all creatures. They can't be regenerated.";

            p.Trigger(new OnStepStart(Step.Upkeep)
              {Condition = (t, g) => g.Players.Permanents().Count(c => c.Is().Creature) >= 4});

            p.Effect = () => new CompoundEffect(
              new SacrificeOwner(),
              new DestroyAllPermanents((e, c) => c.Is().Creature, allowToRegenerate: false));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}