namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class ScuttlingDoomEngine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scuttling Doom Engine")
        .ManaCost("{6}")
        .Type("Artifact Creature — Construct")
        .Text("Scuttling Doom Engine can't be blocked by creatures with power 2 or less.{EOL}When Scuttling Doom Engine dies, it deals 6 damage to target opponent.")
        .FlavorText("A masterwork of spite, inspired by madness.")
        .Power(6)
        .Toughness(6)
        .MinBlockerPower(3)
        .TriggeredAbility(p =>
        {
          p.Text = "When Scuttling Doom Engine dies, it deals 6 damage to target opponent.";
          p.Trigger(new OnZoneChanged(from:Zone.Battlefield, to: Zone.Graveyard));
          p.Effect = () => new DealDamageToPlayer(6, P(e => e.Controller.Opponent));
        });
    }
  }
}
