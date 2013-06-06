namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class SpinedFluke : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Spined Fluke")
        .ManaCost("{2}{B}")
        .Type("Creature Wurm Horror")
        .Text("When Spined Fluke enters the battlefield, sacrifice a creature.{EOL}{B}: Regenerate Spined Fluke.")
        .FlavorText("Its spines are prized as writing quills by the priests of Gix.")
        .Power(5)
        .Toughness(1)
        .OverrideScore(new ScoreOverride {Battlefield = 450})
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "When Spined Fluke enters the battlefield, sacrifice a creature.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PlayerSacrificePermanents(
              count: 1, 
              player: P(e => e.Controller),
              filter: c => c.Is().Creature,
              text: "Sacrifice a creature.");
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{B}: Regenerate Spined Fluke.";
            p.Cost = new PayMana(Mana.Black, ManaUsage.Abilities);
            p.Effect = () => new Gameplay.Effects.Regenerate();
            p.TimingRule(new Artifical.TimingRules.Regenerate());
          });
    }
  }
}