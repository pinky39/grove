namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class GoblinMasons : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Masons")
        .ManaCost("{1}{R}")
        .Type("Creature Goblin")
        .Text("When Goblin Masons dies, destroy target Wall.")
        .FlavorText("Goblins build with true zeal—and anything else within arm's reach.")
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text = "When Goblin Masons dies, destroy target Wall.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is("wall")).On.Battlefield());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}