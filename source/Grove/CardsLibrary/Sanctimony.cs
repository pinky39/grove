namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class Sanctimony : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sanctimony")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("Whenever an opponent taps a Mountain for mana, you may gain 1 life.")
        .FlavorText("To forgive our enemies is to forgive ourselves.")
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent taps a Mountain for mana, you may gain 1 life.";

            p.Trigger(new OnPermanentGetsTapped((a, c) =>
              c.Is("mountain") && c.Controller == a.OwningCard.Controller.Opponent));

            p.Effect = () => new YouGainLife(1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}