namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Vaultbreaker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vaultbreaker")
        .ManaCost("{3}{R}")
        .Type("Creature — Orc Rogue")
        .Text("Whenever Vaultbreaker attacks, you may discard a card. If you do, draw a card.{EOL}Dash {2}{R}{I}(You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.){/I}")
        .Power(4)
        .Toughness(2)
        .Dash("{2}{R}")
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Vaultbreaker attacks, you may discard a card. If you do, draw a card.";
          p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat && dmg.IsDealtByOwningCard && dmg.IsDealtToPlayer));

          p.Effect = () => new DiscardCardToDrawCard();
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
