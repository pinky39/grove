namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class SleeperAgent : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sleeper Agent")
        .ManaCost("{B}")
        .Type("Creature Minion")
        .Text(
          "When Sleeper Agent enters the battlefield, target opponent gains control of it.{EOL}At the beginning of your upkeep, Sleeper Agent deals 2 damage to you.")
        .Power(3)
        .Toughness(3)
        .OverrideScore(new ScoreOverride {Battlefield = 10})
        .TriggeredAbility(p =>
          {
            p.Text = "When Sleeper Agent enters the battlefield, target opponent gains control of it.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new SwitchController();
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, Sleeper Agent deals 2 damage to you.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new DealDamageToPlayer(2, P(e => e.Controller));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}