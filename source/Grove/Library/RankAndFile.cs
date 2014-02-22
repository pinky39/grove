namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class RankAndFile : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rank and File")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Zombie")
        .Text("When Rank and File enters the battlefield, green creatures get -1/-1 until end of turn.")
        .FlavorText("Left, right, left, right . . . hmm. Okay—left, left, left, left, . . .")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text = "When Rank and File enters the battlefield, green creatures get -1/-1 until end of turn.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (_, card) => card.Is().Creature && card.HasColor(CardColor.Green),
              modifiers: () => new AddPowerAndToughness(-1, -1) {UntilEot = true})
              {
                ToughnessReduction = 1
              };
          });
    }
  }
}