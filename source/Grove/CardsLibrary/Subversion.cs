namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class Subversion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Subversion")
        .ManaCost("{3}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, each opponent loses 1 life. You gain life equal to the life lost this way.")
        .FlavorText("Kerrick's corrupt domain swelled like a blister on Tolaria's skin.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, each opponent loses 1 life. You gain life equal to the life lost this way.";

            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ControllerGainsLifeOpponentLoosesLife(1, 1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}