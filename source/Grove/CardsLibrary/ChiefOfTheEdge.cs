namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class ChiefOfTheEdge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chief of the Edge")
        .ManaCost("{W}{B}")
        .Type("Creature - Human Warrior")
        .Text("Other Warrior creatures you control get +1/+0.")
        .FlavorText("\"We are the swift, the strong, the blade's sharp shriek! Fear nothing, and strike!\"")
        .Power(3)
        .Toughness(2)
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 0);
          p.CardFilter = (c, e) =>  c.Is("warrior") && c.Controller == e.Source.Controller && c.Is().Creature && c != e.Source;
        });
    }
  }
}
