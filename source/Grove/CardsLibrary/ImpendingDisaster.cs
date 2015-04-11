namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class ImpendingDisaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Impending Disaster")
        .ManaCost("{1}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, if there are seven or more lands on the battlefield, sacrifice Impending Disaster and destroy all lands.")
        .FlavorText("The goblins are in charge of maintenance? Why not just set it on fire now and call it a day?")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if there are seven or more lands on the battlefield, sacrifice Impending Disaster and destroy all lands.";

            p.Trigger(new OnStepStart(Step.Upkeep)
              {Condition = ctx => ctx.Players.Permanents().Count(c => c.Is().Land) >= 7});

            p.Effect = () => new CompoundEffect(
              new SacrificeOwner(),
              new DestroyAllPermanents((c, ctx) => c.Is().Land));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}