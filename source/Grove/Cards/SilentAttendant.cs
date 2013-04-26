namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class SilentAttendant : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Silent Attendant")
        .Type("Creature Human Cleric")
        .ManaCost("{2}{W}")
        .Text("{T}: You gain 1 life.")
        .FlavorText(
          "'The answer to life should never be death; it should always be more life, wrapped tight around us like precious silks.'")
        .Power(0)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: You gain 1 life.";
            p.Cost = new Tap();
            p.Effect = () => new ControllerGainsLife(1);
            p.TimingRule(new Any(new EndOfTurn(), new OwningCardWillBeDestroyed()));
          });
    }
  }
}