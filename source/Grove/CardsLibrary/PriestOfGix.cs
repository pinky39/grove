namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Grove.Effects;
  using Grove.Triggers;

  public class PriestOfGix : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Priest of Gix")
        .ManaCost("{2}{B}")
        .Type("Creature Human Cleric Minion")
        .Text("When Priest of Gix enters the battlefield, add {B}{B}{B} to your mana pool.")
        .FlavorText("Gix doesn't want a people to rule but puppets to entertain his madness.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text = "When Priest of Gix enters the battlefield, add {B}{B}{B} to your mana pool.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new AddManaToPool("{B}{B}{B}".Parse());
          });
    }
  }
}