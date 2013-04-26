namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class AbyssalHorror : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Abyssal Horror")
        .ManaCost("{4}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Abyssal Horror enters the battlefield, target player discards two cards.")
        .FlavorText("'It has no face of its own—it wears that of its latest victim.'")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Abyssal Horror enters the battlefield, target player discards two cards.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DiscardCards(2);
            p.TargetSelector.AddEffect(s => s.Is.Player());
            p.TargetingRule(new Opponent());
          });
    }
  }
}