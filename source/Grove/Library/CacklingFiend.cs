namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class CacklingFiend : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cackling Fiend")
        .ManaCost("{2}{B}{B}")
        .Type("Creature - Zombie")
        .Text("When Cackling Fiend enters the battlefield, each opponent discards a card.")
        .FlavorText("Its windpipe is only the first to amplify its maddening laughter.")
        .Power(2)
        .Toughness(1)
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "When Cackling Fiend enters the battlefield, each opponent discards a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
          });
    }
  }
}