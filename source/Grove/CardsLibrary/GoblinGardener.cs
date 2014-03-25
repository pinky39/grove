namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class GoblinGardener : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Gardener")
        .ManaCost("{3}{R}")
        .Type("Creature Goblin")
        .Text("When Goblin Gardener dies, destroy target land.")
        .FlavorText("Grow food in dirt? Save time—eat dirt.")
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text = "When Goblin Gardener dies, destroy target land.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}