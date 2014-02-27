namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.Triggers;

  public class AbyssalHorror : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abyssal Horror")
        .ManaCost("{4}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Abyssal Horror enters the battlefield, target player discards two cards.")
        .FlavorText("'It has no face of its own—it wears that of its latest victim.'")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .OverrideScore(new ScoreOverride {Battlefield = 300})
        .TriggeredAbility(p =>
          {
            p.Text = "When Abyssal Horror enters the battlefield, target player discards two cards.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DiscardCards(2);
            p.TargetSelector.AddEffect(s => s.Is.Player());
            p.TargetingRule(new EffectOpponent());
          });
    }
  }
}