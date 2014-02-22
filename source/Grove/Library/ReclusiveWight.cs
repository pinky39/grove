namespace Grove.Library
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class ReclusiveWight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Reclusive Wight")
        .ManaCost("{3}{B}")
        .Type("Creature Zombie Minion")
        .Text("At the beginning of your upkeep, if you control another nonland permanent, sacrifice Reclusive Wight.")
        .FlavorText("There are places so horrible that even the dead hide their faces.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if you control another nonland permanent, sacrifice Reclusive Wight.";

            p.Trigger(new OnStepStart(step: Step.Upkeep)
              {
                Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => !c.Is().Land) > 1
              });

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}