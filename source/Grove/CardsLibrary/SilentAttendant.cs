namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class SilentAttendant : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new ChangeLife(amount: 1, forYou: true);
            p.TimingRule(new Any(new OnEndOfOpponentsTurn(), new WhenOwningCardWillBeDestroyed()));
          });
    }
  }
}