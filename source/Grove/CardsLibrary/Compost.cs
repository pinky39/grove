namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class Compost : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Compost")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text("Whenever a black card is put into an opponent's graveyard from anywhere, you may draw a card.")
        .FlavorText("A fruit must rot before its seed can sprout.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a black card is put into an opponent's graveyard from anywhere, you may draw a card.";

            p.Trigger(new OnZoneChanged(
              to: Zone.Graveyard,
              selector: (c, ctx) => c.Owner == ctx.Opponent && c.HasColor(CardColor.Black)));

            p.Effect = () => new DrawCards(1);
            
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}