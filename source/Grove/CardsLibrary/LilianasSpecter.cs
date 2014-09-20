namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class LilianasSpecter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Liliana's Specter")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Liliana's Specter enters the battlefield, each opponent discards a card.")
        .FlavorText("'The finest minions know what I need without me ever saying a thing.'")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[3])
        .Power(2)
        .Toughness(1)
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Liliana's Specter enters the battlefield, each opponent discards a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
          });
    }
  }
}